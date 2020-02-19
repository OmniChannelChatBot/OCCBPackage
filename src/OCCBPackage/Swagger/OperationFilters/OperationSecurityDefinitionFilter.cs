using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using OCCBPackage.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;

namespace OCCBPackage.Swagger.OperationFilters
{
    internal class OperationSecurityDefinitionFilter : IOperationFilter
    {
        private readonly string _openApiReferenceId;

        public OperationSecurityDefinitionFilter(string openApiReferenceId) =>
            _openApiReferenceId = openApiReferenceId;

        /// <inheritdoc/>
        public void Apply(OpenApiOperation openApiOperation, OperationFilterContext operationFilterContext)
        {
            var isAuthorize = operationFilterContext.ApiDescription.ActionDescriptor.EndpointMetadata
                .Any(s => s is AuthorizeAttribute);

            var isAllowAnonymous = operationFilterContext.ApiDescription.ActionDescriptor.EndpointMetadata
               .Any(s => s is AllowAnonymousAttribute);

            if (isAuthorize && !isAllowAnonymous)
            {
                var schema = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = _openApiReferenceId
                    }
                };

                openApiOperation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement { [schema] = new string[]{ _openApiReferenceId } }
                };

                openApiOperation.Responses.Add(
                    StatusCodes.Status401Unauthorized.ToString(),
                    new OpenApiResponse
                    {
                        Description = Constants.ProblemTypes[StatusCodes.Status401Unauthorized].Item1,
                        Content = new Dictionary<string, OpenApiMediaType>
                        {
                            {
                                MediaTypeNames.Application.Json,
                                new OpenApiMediaType
                                {
                                    Schema = operationFilterContext
                                            .SchemaGenerator
                                            .GenerateSchema(typeof(ApiProblemDetails), operationFilterContext.SchemaRepository)
                                }
                            },
                        }
                    });
            }
        }
    }
}
