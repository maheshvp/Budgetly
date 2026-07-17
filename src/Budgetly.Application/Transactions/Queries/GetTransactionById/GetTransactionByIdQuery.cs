using Budgetly.Application.Common.Results;
using Budgetly.Application.Transactions.Dtos;
using MediatR;

namespace Budgetly.Application.Transactions.Queries.GetTransactionById;

public sealed record GetTransactionByIdQuery(Guid Id) : IRequest<Result<TransactionDto>>;
