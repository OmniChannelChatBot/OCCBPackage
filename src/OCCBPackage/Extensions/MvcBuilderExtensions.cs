using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.DependencyInjection;
using OCCBPackage.Mvc.Filters;
using OCCBPackage.Mvc.ParameterTransformers;

namespace OCCBPackage.Extensions
{
    public static class MvcBuilderExtensions
    {
        public static IMvcBuilder AddApiServices(this IServiceCollection services) => services
            .AddTransient<IValidatorFactory, ServiceProviderValidatorFactory>()
            .AddMvcActionFilters()
            .AddControllers(o =>
            {
                o.Filters.AddService<ApiActionFilter>();
                o.Filters.AddService<ApiResultFilter>();
                o.Conventions.Add(new RouteTokenTransformerConvention(new CamelCaseParameterTransformer()));
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
