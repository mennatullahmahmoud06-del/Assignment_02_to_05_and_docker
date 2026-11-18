using Assignment.Data;
using Assignment.Extensions;
using Assignment.Middlewares;

namespace Assignment
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Inversion of Control (IoC) - Dependency Injection
            builder.Services.AddControllers();
            builder.Services.AddServices(builder.Configuration);

            var app = builder.Build();

            #region Session 02 - Automatic Migration + Seeding
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger<Program>();
                try
                {
                    // Game database
                    var gameDb = services.GetRequiredService<ApplicationDbContext>();
                    await DatabaseInitializer.InitializeAsync(gameDb);
                    await StoreContextSeeding.SeedAsync(gameDb);

                    // Authentication database (Identity)
                    var authDb = services.GetRequiredService<AuthenticationDbContext>();
                    await DatabaseInitializer.InitializeAsync(authDb);
                    await IdentitySeeding.SeedAsync(services, app.Configuration);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Database initialization failed: {Message}", ex.InnerException?.Message ?? ex.Message);
                }
            }
            #endregion

            #region Session 04 - Middlewares (order matters)
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMiddleware<RequestTimingMiddleware>();
            app.UseMiddleware<RequestIdHeaderMiddleware>();
            app.UseMiddleware<RateLimitingMiddleware>();
            #endregion

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            #region Session 06 - CORS + Authentication + Authorization
            app.UseCors(ApplicationServiceExtension.CorsPolicyName);
            app.UseAuthentication();
            app.UseAuthorization();
            #endregion

            app.MapControllers();

            app.Run();
        }
    }
}
