using System.Collections.Concurrent;

namespace Assignment.Middlewares
{
    // Fixed window rate limiter per client IP. Limits come from appsettings (RateLimiting section).
    public class RateLimitingMiddleware
    {
        private static readonly ConcurrentDictionary<string, ClientWindow> Clients = new();

        private sealed class ClientWindow
        {
            public DateTime Start = DateTime.UtcNow;
            public int Count;
        }

        private readonly RequestDelegate _next;
        private readonly int _permitLimit;
        private readonly TimeSpan _window;

        public RateLimitingMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _permitLimit = configuration.GetValue("RateLimiting:PermitLimit", 120);
            _window = TimeSpan.FromSeconds(configuration.GetValue("RateLimiting:WindowSeconds", 60));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var key = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var client = Clients.GetOrAdd(key, _ => new ClientWindow());

            bool limited;
            double retryAfterSeconds;

            lock (client)
            {
                if (DateTime.UtcNow - client.Start > _window)
                {
                    client.Start = DateTime.UtcNow;
                    client.Count = 0;
                }

                client.Count++;
                limited = client.Count > _permitLimit;
                retryAfterSeconds = Math.Ceiling((client.Start + _window - DateTime.UtcNow).TotalSeconds);
            }

            if (limited)
            {
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                context.Response.Headers["Retry-After"] = Math.Max(retryAfterSeconds, 1).ToString();
                await context.Response.WriteAsJsonAsync(new { error = "Rate limit exceeded. Please slow down." });
                return;
            }

            await _next(context);
        }
    }
}
