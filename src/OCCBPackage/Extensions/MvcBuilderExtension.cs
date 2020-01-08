using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using OCCBPackage.Mvc.Filters;

namespace OCCBPackage.Extensions
{
    public static class MvcBuilderExtension
    {
        public static IMvcBuilder AddApiServices(this IServiceCollection services) => services
            .AddTransient<IValidatorFactory, ServiceProviderValidatorFactory>()
            .AddMvcActionFilters()
            .AddControllers(o =>
            {
                o.Filters.AddService<ApiActionFilter>();
                o.Filters.AddService<ApiResultFilter>();
            })
            .AddJsonOptions(o => o.JsonSerializerOptions.IgnoreNullValues = true)
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
                options.SuppressMapClientErrors = true;
            });

        private static IServiceCollection AddMvcActionFilters(this IServiceCollection services) => services
            .AddScoped<ApiActionFilter>()
            .AddScoped<ApiResultFilter>();
    }
}
