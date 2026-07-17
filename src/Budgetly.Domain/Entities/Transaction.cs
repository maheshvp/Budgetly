using Budgetly.Domain.Enums;
using Budgetly.Domain.ValueObjects;

namespace Budgetly.Domain.Entities;

public sealed class Transaction
{
    public Guid Id { get; private set; }
    public Guid AccountId { get; private set; }
    public Guid CategoryId { get; private set; }
    public Money Amount { get; private set; } = null!;
    public TransactionType Type { get; private set; }
    public DateOnly Date { get; private set; }
    public string? Note { get; private set; }

    private Transaction() { }

    public static Transaction Create(
        Guid accountId,
        Guid categoryId,
        Money amount,
        TransactionType type,
        DateOnly date,
        string? note)
    {
        if (amount.Amount <= 0m)
            throw new ArgumentException("Transaction amount must be greater than zero.", nameof(amount));

        if (note is not null && note.Length > 250)
            throw new ArgumentException("Note must not exceed 250 characters.", nameof(note));

        return new Transaction
        {
            Id = Guid.NewGuid(),
            AccountId = accountId,
            CategoryId = categoryId,
            Amount = amount,
            Type = type,
            Date = date,
            Note = note
        };
    }
}
