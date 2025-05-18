using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace PizzeriaApi.Extensions
{
    public static class JwtExtension
    {

        public static IServiceCollection AddAuthenticationExtension(this IServiceCollection services, string issuer, string audience, string signingKey)
        {
            if (string.IsNullOrWhiteSpace(issuer))
                throw new ArgumentException("JWT issuer cannot be null or empty.", nameof(issuer));
            if (string.IsNullOrWhiteSpace(audience))
                throw new ArgumentException("JWT audience cannot be null or empty.", nameof(audience));
            if (string.IsNullOrWhiteSpace(signingKey))
                throw new ArgumentException("JWT signing key cannot be null or empty.", nameof(signingKey));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
                        RoleClaimType = ClaimTypes.Role,
                        NameClaimType = ClaimTypes.NameIdentifier
                    };

                    

                });

            return services;
        }

    }
}
