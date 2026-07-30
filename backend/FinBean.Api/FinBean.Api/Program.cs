using FinBean.Api.Authentication;
using FinBean.Core.Authentication;
using FinBean.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace FinBean.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        // Enables ProblemDetails responses for unhandled exceptions (UseExceptionHandler) and for
        // empty error responses such as 401/403/404 (UseStatusCodePages). [ApiController] already
        // returns ProblemDetails for model-validation failures.
        builder.Services.AddProblemDetails();

        builder.Services.AddDbContext<FinBeanDbContext>(options =>
            options.UseNpgsql(GetRequiredConnectionString(builder.Configuration)));

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => ConfigureJwtBearer(options, builder.Configuration, builder.Environment.IsDevelopment()));

        builder.Services.AddAuthorization();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.UseExceptionHandler();
        app.UseStatusCodePages();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

    private static string GetRequiredConnectionString(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("FinBeanDb");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Missing configuration value 'ConnectionStrings:FinBeanDb'. Set it via 'dotnet user-secrets', " +
                "the ConnectionStrings__FinBeanDb environment variable, or appsettings.Development.json. " +
                "See backend/.env.template for the expected shape.");
        }

        return connectionString;
    }

    private static void ConfigureJwtBearer(JwtBearerOptions options, IConfiguration configuration, bool isDevelopment)
    {
        var authority = configuration["Keycloak:Authority"];
        var audience = configuration["Keycloak:Audience"];

        if (string.IsNullOrWhiteSpace(authority) || string.IsNullOrWhiteSpace(audience))
        {
            throw new InvalidOperationException(
                "Missing configuration values 'Keycloak:Authority' / 'Keycloak:Audience'. Set them via " +
                "'dotnet user-secrets', the Keycloak__Authority / Keycloak__Audience environment variables, " +
                "or appsettings.Development.json. See backend/.env.template for the expected shape.");
        }

        options.Authority = authority;
        options.Audience = audience;
        options.MetadataAddress = configuration["Keycloak:MetadataAddress"]
            ?? $"{authority}/.well-known/openid-configuration";
        // The local Keycloak dev server (docker/docker-compose.yml) runs over plain HTTP.
        options.RequireHttpsMetadata = !isDevelopment;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = authority,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateLifetime = true,
        };
    }
}