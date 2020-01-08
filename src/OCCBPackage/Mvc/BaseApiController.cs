using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace OCCBPackage.Mvc
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public abstract class BaseApiController : ControllerBase
    {
    }
}
