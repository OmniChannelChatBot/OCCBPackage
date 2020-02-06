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
        public static IServiceCollection AddJwtBearerAuthentication(this IServiceCollection services, Action<AccessTokenOptions> accessTokenOptions, Action<RefreshTokenOptions> refreshTokenOptions) => services
            .AddJwtBearerAuthentication(accessTokenOptions)
            .Configure(refreshTokenOptions);

        public static IServiceCollection AddJwtBearerAuthentication(this IServiceCollection services, Action<AccessTokenOptions> accessTokenOptions)
        {
            services.Configure(accessTokenOptions);
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
                        // TODO: When using WebSockets and server events, 
                        //       the token is passed as a parameter of the query string with the access_token key
                        //       https://docs.microsoft.com/ru-ru/aspnet/core/signalr/authn-and-authz?view=aspnetcore-3.1#bearer-token-authentication
                        var accessToken = context.Request.Query["access_token"].ToString();

                        if (!string.IsNullOrEmpty(accessToken))
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
