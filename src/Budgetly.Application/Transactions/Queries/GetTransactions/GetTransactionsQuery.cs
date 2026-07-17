using Budgetly.Application.Common.Results;
using Budgetly.Application.Transactions.Dtos;
using MediatR;

namespace Budgetly.Application.Transactions.Queries.GetTransactions;

public sealed record GetTransactionsQuery(Guid? AccountId, DateOnly? From, DateOnly? To)
    : IRequest<Result<IReadOnlyList<TransactionDto>>>;
