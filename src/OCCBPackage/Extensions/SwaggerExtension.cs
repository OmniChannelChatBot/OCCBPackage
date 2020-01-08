using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace OCCBPackage.Extensions
{
    public static class SwaggerExtension
    {
        private const string RoutePrefix = "api-doc";
        private const string DefaultVersion = "v1";

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
                    Version = DefaultVersion,
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
            .UseSwagger(options => options.RouteTemplate = $"{RoutePrefix}/{{documentName}}/swagger.json")
            .UseSwaggerUI(c =>
            {
                var serviceName = Environments.GetServiceName();
                var endpointName = $"{serviceName} {DefaultVersion}";

                c.DocumentTitle = $"{serviceName} api docs";
                c.SwaggerEndpoint($"/{RoutePrefix}/{serviceName}/swagger.json", endpointName);
                c.RoutePrefix = RoutePrefix;
                c.DisplayRequestDuration();
                c.DisplayOperationId();
            });
    }
}
