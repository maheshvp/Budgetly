using Budgetly.Application.Common.Interfaces;
using Budgetly.Application.Common.Results;
using Budgetly.Domain.Entities;
using Budgetly.Domain.ValueObjects;
using MediatR;

namespace Budgetly.Application.Transactions.Commands.CreateTransaction;

public sealed class CreateTransactionHandler(
    IAccountRepository accountRepository,
    ICategoryRepository categoryRepository,
    ITransactionRepository transactionRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateTransactionCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        CreateTransactionCommand command,
        CancellationToken cancellationToken)
    {
        var account = await accountRepository.GetByIdAsync(command.AccountId, cancellationToken);
        if (account is null)
            return Result.Failure<Guid>(Error.NotFound(nameof(Account), command.AccountId));

        var category = await categoryRepository.GetByIdAsync(command.CategoryId, cancellationToken);
        if (category is null)
            return Result.Failure<Guid>(Error.NotFound(nameof(Category), command.CategoryId));

        if (!command.Currency.Equals(account.Balance.Currency, StringComparison.OrdinalIgnoreCase))
            return Result.Failure<Guid>(Error.Validation(
                nameof(command.Currency),
                $"Transaction currency '{command.Currency}' does not match account currency '{account.Balance.Currency}'."));

        if ((int)command.Type != (int)category.Type)
            return Result.Failure<Guid>(Error.Validation(
                nameof(command.Type),
                $"Transaction type '{command.Type}' does not match category type '{category.Type}'."));

        var money = new Money(command.Amount, command.Currency);
        var transaction = Transaction.Create(
            command.AccountId,
            command.CategoryId,
            money,
            command.Type,
            command.Date,
            command.Note);

        account.Apply(transaction);

        await transactionRepository.AddAsync(transaction, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(transaction.Id);
    }
}
