using Budgetly.Application.Common.Results;
using Budgetly.Domain.Enums;
using MediatR;

namespace Budgetly.Application.Transactions.Commands.CreateTransaction;

public sealed record CreateTransactionCommand(
    Guid AccountId,
    Guid CategoryId,
    decimal Amount,
    string Currency,
    TransactionType Type,
    DateOnly Date,
    string? Note) : IRequest<Result<Guid>>;
