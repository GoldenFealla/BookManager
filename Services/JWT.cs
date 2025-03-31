using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace BookManager.Services
{
    public static class RSAKeyloader
    {
        public static RSA LoadKey(string path)
        {
            RSA key = RSA.Create();
            string pem = File.ReadAllText(path);
            key.ImportFromPem(pem);
            return key;
        }
    }

    public class JWTService(string Issuer, string Audience, RsaSecurityKey RsaSecurityKey)
    {
        private readonly string _issuer = Issuer;
        private readonly string _audience = Audience;
        private readonly int _expiryInMinutes = 5;
        private readonly RsaSecurityKey _rsaSecurityKey = RsaSecurityKey;

        public string GenerateJwtToken(string username)
        {
            // Create signing credentials using the RSA private key
            var credentials = new SigningCredentials(_rsaSecurityKey, SecurityAlgorithms.RsaSha256);

            // Create user identity claims
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, username == "admin" ? "Admin" : "User"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
            };

            // Create the JWT token
            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_expiryInMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public static class JWTExtensions
    {
        public static AuthenticationBuilder AddJWTAuthentication(
            this IServiceCollection services, string Issuer, string Audience, RSA publicKey)
        {
            AuthenticationBuilder AuthenticationBuilder = services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.IncludeErrorDetails = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Issuer,
                    ValidAudience = Audience,
                    IssuerSigningKey = new RsaSecurityKey(publicKey),
                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Append("Token-Expired", "true");
                        }
                        else
                        {
                            Console.WriteLine(context.Exception);
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            return AuthenticationBuilder;
        }

        public static AuthorizationBuilder AddJWTAuthorization(this IServiceCollection services, RSA privateKey)
        {
            AuthorizationBuilder AuthorizationBuilder = services.AddAuthorizationBuilder()
                .AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"))
                .AddPolicy("UserOnly", policy => policy.RequireRole("User"));

            services.AddSingleton(provider => new RsaSecurityKey(privateKey));

            return AuthorizationBuilder;
        }
    }
}