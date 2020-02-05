using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OCCBPackage.Mvc;
using OCCBPackage.Options;
using System;
using System.Net.Mime;
using System.Security.Authentication;
using System.Text.Json;
using System.Threading.Tasks;

namespace OCCBPackage.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJwtBearerAuthentication(this IServiceCollection services, Action<AccessTokenOptions> accessTokenOptions, Action<RefreshTokenOptions> refreshTokenOptions = default)
        {
            services.Configure(accessTokenOptions);

            if (refreshTokenOptions != default)
            {
                services.Configure(refreshTokenOptions);
            }

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                var accessTokenOptions = services
                    .BuildServiceProvider()
                    .GetRequiredService<IOptions<AccessTokenOptions>>();

                x.ClaimsIssuer = accessTokenOptions.Value.Issuer;
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = accessTokenOptions.Value.GetTokenValidationParameters();
                x.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        if (!string.IsNullOrEmpty(accessToken)
                            && (context.HttpContext.Request.Path.StartsWithSegments("/chat")))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        if (context.AuthenticateFailure is SecurityTokenExpiredException)
                        {
                            context.Response.Headers.Add("Token-Expired", JwtBearerDefaults.AuthenticationScheme);
                        }

                        context.Response.Headers.Add("WWW-Authenticate", JwtBearerDefaults.AuthenticationScheme);
                        context.Response.ContentType = MediaTypeNames.Application.Json;
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                        var response = JsonSerializer.Serialize(new ApiProblemDetails(context.HttpContext), new JsonSerializerOptions
                        {
                            IgnoreNullValues = true,
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        });

                        return context.Response.WriteAsync(response);
                    }
                };
            });

            return services;
        }
    }
}
