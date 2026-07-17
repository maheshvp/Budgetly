using Budgetly.Application.Categories.Commands.CreateCategory;
using Budgetly.Application.Categories.Queries.GetCategories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Budgetly.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class CategoriesController(ISender sender) : ApiControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateCategory(
        CreateCategoryCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return ToErrorResponse(result.Error);

        return StatusCode(StatusCodes.Status201Created, new { id = result.Value });
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetCategoriesQuery(), cancellationToken);

        return Ok(result.Value);
    }
}
