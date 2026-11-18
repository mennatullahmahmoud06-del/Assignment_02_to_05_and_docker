using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Assignment.Constants;
using Assignment.Filters;
using Assignment.Results;

namespace Assignment.Controllers
{
    // Session 05: the logging + timing filters are applied to every controller through this base class
    [ApiController]
    [ServiceFilter(typeof(ActionExecutionLoggingFilter))]
    [ServiceFilter(typeof(ActionTimingFilter))]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        protected bool IsAdmin => User.IsInRole(Roles.Admin);

        protected IActionResult HandleResult<T>(GenericResult<T> result)
            => result.IsSuccess ? Ok(result) : Failure(result);

        protected IActionResult HandleResult(Result result)
            => result.IsSuccess ? NoContent() : Failure(result);

        private IActionResult Failure(Result result) => result.Kind switch
        {
            ErrorKind.NotFound => NotFound(result),
            ErrorKind.Conflict => Conflict(result),
            ErrorKind.Forbidden => StatusCode(StatusCodes.Status403Forbidden, result),
            _ => BadRequest(result)
        };
    }
}
