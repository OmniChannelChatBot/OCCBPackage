using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace OCCBPackage.Extensions
{
    public static class SwaggerExtension
    {
        private static readonly string _routePrefix = "api-doc";
        private static readonly string _defaultVersion = "v1";

        public static IServiceCollection AddCustomSwagger(this IServiceCollection services, Action<SwaggerGenOptions> swaggerGenOptions = default)
        {
            if (swaggerGenOptions != default)
            {
                services.Configure(swaggerGenOptions);
            }

            services.AddSwaggerGen(c =>
            {
                var serviceName = Environments.GetServiceName();

                c.SwaggerDoc(serviceName, new OpenApiInfo
                {
                    Version = _defaultVersion,
                    Title = serviceName,
                    Description = $"`{serviceName}`",
                    Contact = new OpenApiContact
                    {
                        Name = "OmniChannelChatBot",
                        Url = new Uri("https://github.com/OmniChannelChatBot")
                    }
                });

                c.EnableAnnotations();
            });

            return services;
        }

        public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app) => app
            .UseSwagger(options => options.RouteTemplate = $"{_routePrefix}/{{documentName}}/swagger.json")
            .UseSwaggerUI(c =>
            {
                var serviceName = Environments.GetServiceName();
                var endpointName = $"{serviceName} {_defaultVersion}";

                c.DocumentTitle = $"{serviceName} api docs";
                c.SwaggerEndpoint($"/{_routePrefix}/{serviceName}/swagger.json", endpointName);
                c.RoutePrefix = _routePrefix;
                c.DisplayRequestDuration();
                c.DisplayOperationId();
            });
    }
}
