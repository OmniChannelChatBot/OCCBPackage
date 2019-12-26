using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace OCCBPackage.Filters
{
    internal class ApiResultFilter : IAsyncResultFilter
    {
        public Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            switch (context.Result)
            {
                case NotFoundObjectResult nfor:
                    context.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                    ResultApiProblemDetails(context, nfor.Value.ToString());
                    break;

                case BadRequestObjectResult bror:
                    context.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                    ResultApiProblemDetails(context, bror.Value.ToString());
                    break;

                case NotFoundResult _:
                    context.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                    ResultApiProblemDetails(context);
                    break;
            }

            return next();
        }

        private void ResultApiProblemDetails(ResultExecutingContext resultExecutingContext, string title = default(string))
        {
            var apiProblemDetails = new ApiProblemDetails(resultExecutingContext.HttpContext, title);
            var objectResult = new ObjectResult(apiProblemDetails)
            {
                StatusCode = resultExecutingContext.HttpContext.Response.StatusCode
            };
            resultExecutingContext.Result = objectResult;
        }
    }
}
