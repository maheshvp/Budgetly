using Budgetly.Application.Common.Results;
using MediatR;

namespace Budgetly.Application.Accounts.Commands.CreateAccount;

public sealed record CreateAccountCommand(string Name, string Currency) : IRequest<Result<Guid>>;
