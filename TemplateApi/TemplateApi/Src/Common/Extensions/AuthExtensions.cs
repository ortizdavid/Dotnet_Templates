using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TemplateApi.Core.Services.Auth;

namespace TemplateApi.Common.Extensions;

public static class AuthExtensions
{
    public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)  
    {
        // Register the JWTSettings from appsettings.json
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            // Access the JwtSettings from the configuration
            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
            var secretkey = jwtSettings?.SecretKey ?? string.Empty;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings?.Issuer,
                ValidAudience = jwtSettings?.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretkey))
            };
        });
    }
}
