namespace Assignment.Middlewares
{
    // Factory-based middleware (IMiddleware): it is registered in DI with AddTransient
    public class RequestIdHeaderMiddleware(ILogger<RequestIdHeaderMiddleware> logger) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var requestId = context.TraceIdentifier;
            context.Response.Headers["X-Request-Id"] = requestId;

            logger.LogDebug(
                "Assigned request ID {RequestId} to {Method} {Path}",
                requestId,
                context.Request.Method,
                context.Request.Path);

            await next(context);
        }
    }
}
