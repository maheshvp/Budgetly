using Budgetly.Application.Common.Interfaces;
using Budgetly.Application.Common.Results;
using Budgetly.Domain.Entities;
using MediatR;

namespace Budgetly.Application.Accounts.Commands.CreateAccount;

public sealed class CreateAccountHandler(
    IAccountRepository accountRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateAccountCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        CreateAccountCommand command,
        CancellationToken cancellationToken)
    {
        var account = Account.Create(command.Name, command.Currency);

        await accountRepository.AddAsync(account, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(account.Id);
    }
}
