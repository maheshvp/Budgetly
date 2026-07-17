using Budgetly.Domain.Entities;
using Budgetly.Domain.Enums;

namespace Budgetly.Application.Transactions.Dtos;

public sealed record TransactionDto(
    Guid Id,
    Guid AccountId,
    Guid CategoryId,
    decimal Amount,
    string Currency,
    TransactionType Type,
    DateOnly Date,
    string? Note)
{
    public static TransactionDto FromEntity(Transaction transaction) =>
        new(
            transaction.Id,
            transaction.AccountId,
            transaction.CategoryId,
            transaction.Amount.Amount,
            transaction.Amount.Currency,
            transaction.Type,
            transaction.Date,
            transaction.Note);
}
