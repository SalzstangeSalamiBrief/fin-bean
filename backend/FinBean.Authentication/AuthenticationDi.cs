using FinBean.Authentication.Options;
using FinBean.Authentication.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FinBean.Authentication;

public static class AuthenticationDi
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddKeycloakOptions()
        {
            services
                .AddOptions<KeycloakOptions>()
                .BindConfiguration(nameof(KeycloakOptions))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            return services;
        }

        public IServiceCollection AddKeycloakAuthentication()
        {
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer();

            // Configure JwtBearerOptions from the strongly typed KeycloakOptions (options pattern)
            // instead of reading IConfiguration directly.
            services
                .AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
                .Configure<IOptions<KeycloakOptions>>((jwtBearerOptions, keycloakOptions) =>
                {
                    var options = keycloakOptions.Value;

                    jwtBearerOptions.Authority = options.Authority;
                    jwtBearerOptions.RequireHttpsMetadata = options.RequireHttpsMetadata;
                    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = options.Authority,
                        ValidateAudience = true,
                        ValidAudience = options.Audience,
                        ValidateLifetime = true,
                    };
                });

            services.AddAuthorization();

            return services;
        }

        public IServiceCollection AddCurrentUserService()
        {
            services.AddHttpContextAccessor();
            services.AddScoped<CurrentUserService>();

            return services;
        }
    }
}