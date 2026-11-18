using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;
using Assignment.Data;
using Assignment.Data.Interceptors;
using Assignment.Entities;
using Assignment.Filters;
using Assignment.GameLogic;
using Assignment.Helper;
using Assignment.Middlewares;
using Assignment.Repositories;
using Assignment.Services;

namespace Assignment.Extensions
{
    public static class ApplicationServiceExtension
    {
        public const string CorsPolicyName = "ConfigurationOrigins";

        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            #region Swagger (with the Bearer button for JWT)
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your JWT token. Swagger will add the Bearer prefix."
                });

                options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer", document)] = []
                });
            });
            #endregion

            #region Session 02 - EF Core (game database) + Session 04 Interceptors
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultSqlConnection"))
                       .AddInterceptors(new SoftDeleteInterceptor(), new CreatedAtInterceptor(), new ModifiedAtInterceptor())
            );

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<HeroClassProfile>();
                cfg.AddProfile<HeroProfile>();
                cfg.AddProfile<QuestProfile>();
            });
            #endregion

            #region Session 03 - Repositories + Services
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IHeroClassRepository, HeroClassRepository>();
            services.AddScoped<IHeroRepository, HeroRepository>();
            services.AddScoped<IHeroQuestRepository, HeroQuestRepository>();

            services.AddScoped<IHeroClassService, HeroClassService>();
            services.AddScoped<IHeroService, HeroService>();
            services.AddScoped<IQuestService, QuestService>();
            services.AddScoped<IGameService, GameService>();
            services.AddSingleton<IDice, RandomDice>();
            #endregion

            #region Session 04 - Middlewares
            services.AddTransient<RequestIdHeaderMiddleware>();
            #endregion

            #region Session 05 - Filters
            services.AddScoped<ActionExecutionLoggingFilter>();
            services.AddScoped<ActionTimingFilter>();
            services.AddScoped<ExceptionHandlingFilter>();
            #endregion

            #region Session 06 - Authentication (Identity + JWT)
            services.AddDbContext<AuthenticationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("AuthenticationConnection"))
            );

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AuthenticationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                }
            );

            services.AddScoped<IJwtService, JwtService>();
            #endregion

            #region Session 06 - CORS
            var corsOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();

            services.AddCors(options =>
                options.AddPolicy(CorsPolicyName, policy =>
                {
                    if (corsOrigins.Length > 0)
                        policy.WithOrigins(corsOrigins);
                    else
                        policy.AllowAnyOrigin();

                    policy.AllowAnyMethod().AllowAnyHeader();
                })
            );
            #endregion

            return services;
        }
    }
}
