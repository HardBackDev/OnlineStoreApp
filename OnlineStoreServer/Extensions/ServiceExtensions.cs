using AspNetCore.Identity.Dapper.Models;
using Contracts.RepositoryContracts;
using Microsoft.AspNetCore.Identity;
using CompanyEmployees.Migrations;
using Contracts.ServiceContracts;
using AspNetCore.Identity.Dapper;
using FluentMigrator.Runner;
using System.Reflection;
using LoggerService;
using Repository;
using Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Repository.EntityRepositories;
using OnlineStoreServer.Presentation.Filtres;
using Microsoft.OpenApi.Models;
using Microsoft.Net.Http.Headers;
using OnlineStoreServer.Migrations;
using OnlineStoreServer.Presentation.Cotrollers;

namespace OnlineStoreServer.Extensions
{
    public static class ServiceExtensions
    {
        static string connection = "sqlConnection";

        public static void ConfigureControllers(this IServiceCollection services)
        {
            services.AddControllers()
                .AddApplicationPart(typeof(OnlineStoreServer.Presentation.AssemblyReference).Assembly);
        }
        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["secret"];
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["validIssuer"],
                    ValidAudience = jwtSettings["validAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });
        }

        public static void ConfigureIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            
            services.AddIdentity<ApplicationUser, ApplicationRole>(o =>
            {
                o.Password.RequireDigit = true;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 10;
                o.User.RequireUniqueEmail = true;
            })
            .AddDapperStores(opt =>
            {
                opt.ConnectionString = configuration.GetConnectionString(connection);
            })
            .AddDefaultTokenProviders();
        }

        public static void ConfigureFluentMigrator(this IServiceCollection services,
            IConfiguration configuration) => services.AddLogging(c =>
            c.AddFluentMigratorConsole())
            .AddFluentMigratorCore().ConfigureRunner(c =>
            c.AddSqlServer2016().WithGlobalConnectionString(configuration
            .GetConnectionString(connection))
            .ScanIn(Assembly.GetExecutingAssembly())
            .For.Migrations());

        public static void ConfigureCors(this IServiceCollection services) =>
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders("X-Pagination"));
        });

        public static void ConfigureIISIntegration(this IServiceCollection services) =>
        services.Configure<IISOptions>(options =>
        {
        });

        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
            services.AddScoped<DapperContext>();
            services.AddTransient<CreatingCategories>();
            services.AddTransient<Database>();
            services.AddTransient<AuthenticationController>();
            services.AddScoped<IRepositoryManager, RepositoryManager>();
            services.AddScoped<IServiceManager, ServiceManager>();
            services.AddScoped<ICartRepository, CartRepository>();
            
        }
    }
}
