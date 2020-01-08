using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using OCCBPackage.Swagger.OperationFilters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace OCCBPackage.Extensions
{
    public static class SwaggerGenOptionsExtension
    {
        public static void AddBearerSecurityDefinition(this SwaggerGenOptions options)
        {
            var scheme = JwtBearerDefaults.AuthenticationScheme;

            options.AddSecurityDefinition(scheme, new OpenApiSecurityScheme
            {
                Description = $"{JwtBearerDefaults.AuthenticationScheme} authentication",
                Type = SecuritySchemeType.Http,
                Scheme = scheme.ToLower(),
                BearerFormat = "JWT"
            });

            options.OperationFilter<OperationSecurityDefinitionFilter>(scheme);
        }
    }
}
