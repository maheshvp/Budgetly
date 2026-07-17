using Budgetly.Application.Common.Results;
using Microsoft.AspNetCore.Mvc;

namespace Budgetly.Api.Controllers;

public abstract class ApiControllerBase : ControllerBase
{
    protected IActionResult ToErrorResponse(Error error)
    {
        var status = error.Code.EndsWith(".NotFound", StringComparison.Ordinal)
            ? StatusCodes.Status404NotFound
            : StatusCodes.Status400BadRequest;

        return Problem(detail: error.Message, statusCode: status, title: error.Code);
    }
}
