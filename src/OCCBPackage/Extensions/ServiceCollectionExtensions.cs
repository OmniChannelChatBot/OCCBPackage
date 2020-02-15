using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using OCCBPackage.Options;
using System;

namespace OCCBPackage.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private static readonly string _corsPolicy = nameof(_corsPolicy);

        public static IServiceCollection AddCookiePolicy(this IServiceCollection services, Action<CookiePolicyOptions> actionCookiePolicyOptions) => services
            .Configure(actionCookiePolicyOptions);

        public static IServiceCollection AddCorsPolicy(this IServiceCollection services, Action<CorsPolicyOptions> action)
        {
            var clientOriginPolicyOptions = new CorsPolicyOptions();
            action.Invoke(clientOriginPolicyOptions);

            services.AddCors(sa => sa
                .AddPolicy(_corsPolicy, cpb =>
                {
                    cpb.WithOrigins(clientOriginPolicyOptions.Origins);
                    cpb.WithMethods(clientOriginPolicyOptions.Methods);
                    cpb.WithHeaders(clientOriginPolicyOptions.Headers);
                    if (clientOriginPolicyOptions.IsCredentials)
                    {
                        cpb.AllowCredentials();
                    }
                }));

            return services;
        }
    }
}
