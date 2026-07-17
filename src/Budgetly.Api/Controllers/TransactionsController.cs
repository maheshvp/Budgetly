using Budgetly.Application.Transactions.Commands.CreateTransaction;
using Budgetly.Application.Transactions.Queries.GetTransactionById;
using Budgetly.Application.Transactions.Queries.GetTransactions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Budgetly.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class TransactionsController(ISender sender) : ApiControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateTransaction(
        CreateTransactionCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return ToErrorResponse(result.Error);

        return CreatedAtAction(nameof(GetTransactionById), new { id = result.Value }, new { id = result.Value });
    }

    [HttpGet]
    public async Task<IActionResult> GetTransactions(
        [FromQuery] Guid? accountId,
        [FromQuery] DateOnly? from,
        [FromQuery] DateOnly? to,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetTransactionsQuery(accountId, from, to), cancellationToken);

        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTransactionById(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetTransactionByIdQuery(id), cancellationToken);

        if (result.IsFailure)
            return ToErrorResponse(result.Error);

        return Ok(result.Value);
    }
}
