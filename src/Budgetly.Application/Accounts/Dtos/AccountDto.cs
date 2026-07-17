using Budgetly.Domain.Entities;

namespace Budgetly.Application.Accounts.Dtos;

public sealed record AccountDto(Guid Id, string Name, decimal Balance, string Currency)
{
    public static AccountDto FromEntity(Account account) =>
        new(account.Id, account.Name, account.Balance.Amount, account.Balance.Currency);
}
