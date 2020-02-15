using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace OCCBPackage.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCookiePolicy(this IServiceCollection services, Action<CookiePolicyOptions> actionCookiePolicyOptions) => services
            .Configure(actionCookiePolicyOptions);
    }
}
