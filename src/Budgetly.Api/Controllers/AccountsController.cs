using Budgetly.Application.Accounts.Commands.CreateAccount;
using Budgetly.Application.Accounts.Queries.GetAccountById;
using Budgetly.Application.Accounts.Queries.GetAccounts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Budgetly.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AccountsController(ISender sender) : ApiControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateAccount(
        CreateAccountCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return ToErrorResponse(result.Error);

        return CreatedAtAction(nameof(GetAccountById), new { id = result.Value }, new { id = result.Value });
    }

    [HttpGet]
    public async Task<IActionResult> GetAccounts(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetAccountsQuery(), cancellationToken);

        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAccountById(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetAccountByIdQuery(id), cancellationToken);

        if (result.IsFailure)
            return ToErrorResponse(result.Error);

        return Ok(result.Value);
    }
}
