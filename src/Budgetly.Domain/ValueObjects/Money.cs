namespace Budgetly.Domain.ValueObjects;

public sealed record Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency)
    {
        if (string.IsNullOrWhiteSpace(currency) || currency.Length != 3 || !currency.All(char.IsLetter))
            throw new ArgumentException("Currency must be a 3-letter ISO code.", nameof(currency));

        Amount = amount;
        Currency = currency.ToUpperInvariant();
    }

    public Money Add(Money other)
    {
        if (!Currency.Equals(other.Currency, StringComparison.Ordinal))
            throw new InvalidOperationException(
                $"Cannot add money with different currencies: '{Currency}' and '{other.Currency}'.");

        return new Money(Amount + other.Amount, Currency);
    }

    public Money Subtract(Money other)
    {
        if (!Currency.Equals(other.Currency, StringComparison.Ordinal))
            throw new InvalidOperationException(
                $"Cannot subtract money with different currencies: '{Currency}' and '{other.Currency}'.");

        return new Money(Amount - other.Amount, Currency);
    }

    public override string ToString() => $"{Amount:F2} {Currency}";
}
