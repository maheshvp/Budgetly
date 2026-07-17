using Budgetly.Domain.Enums;
using Budgetly.Domain.ValueObjects;

namespace Budgetly.Domain.Entities;

public sealed class Account
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public Money Balance { get; private set; } = null!;
    public DateTime CreatedAtUtc { get; private set; }

    private Account() { }

    public static Account Create(string name, string currency)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Account name is required.", nameof(name));

        if (name.Length > 100)
            throw new ArgumentException("Account name must not exceed 100 characters.", nameof(name));

        return new Account
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            Balance = new Money(0m, currency),
            CreatedAtUtc = DateTime.UtcNow
        };
    }

    public void Apply(Transaction transaction)
    {
        if (!transaction.Amount.Currency.Equals(Balance.Currency, StringComparison.Ordinal))
            throw new InvalidOperationException(
                $"Transaction currency '{transaction.Amount.Currency}' does not match " +
                $"account currency '{Balance.Currency}'.");

        Balance = transaction.Type switch
        {
            TransactionType.Income  => Balance.Add(transaction.Amount),
            TransactionType.Expense => Balance.Subtract(transaction.Amount),
            _ => throw new InvalidOperationException($"Unknown transaction type: {transaction.Type}.")
        };
    }
}
