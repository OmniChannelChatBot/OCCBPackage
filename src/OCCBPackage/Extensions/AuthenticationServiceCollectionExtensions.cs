using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OCCBPackage.Mvc;
using OCCBPackage.Options;
using System;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;

namespace OCCBPackage.Extensions
{
    public static class AuthenticationServiceCollectionExtensions
    {
        public static AuthenticationBuilder AddJwtBearerAuthentication(this IServiceCollection services, Action<AccessTokenOptions> actionAccessTokenOptions, Action<RefreshTokenOptions> actionRefreshTokenOptions) => services
            .Configure(actionRefreshTokenOptions)
            .AddJwtBearerAuthentication(actionAccessTokenOptions);

        public static AuthenticationBuilder AddJwtBearerAuthentication(this IServiceCollection services, Action<AccessTokenOptions> actionAccessTokenOptions) => services
            .Configure(actionAccessTokenOptions)
            .AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                var accessTokenOptions = new AccessTokenOptions();
                actionAccessTokenOptions.Invoke(accessTokenOptions);

                x.ClaimsIssuer = accessTokenOptions.Issuer;
                x.RequireHttpsMetadata = accessTokenOptions.IsTransferTokenOverHttps;
                x.SaveToken = accessTokenOptions.IsStoreTokenAfterAuthentication;
                x.TokenValidationParameters = accessTokenOptions.GetTokenValidationParameters();
                x.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = default(string);

                        accessToken = context.Request.Cookies[AccessTokenOptions.TokenName];

                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            context.Token = accessToken;
                            return Task.CompletedTask;
                        }

                        // TODO: When using WebSockets and server events, 
                        //       the token is passed as a parameter of the query string with the access_token key
                        //       https://docs.microsoft.com/ru-ru/aspnet/core/signalr/authn-and-authz?view=aspnetcore-3.1#bearer-token-authentication
                        accessToken = context.Request.Query[AccessTokenOptions.TokenName].ToString();

                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            context.Token = accessToken;
                            return Task.CompletedTask;
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
    }
}
