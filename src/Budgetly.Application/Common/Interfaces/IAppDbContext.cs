namespace Budgetly.Application.Common.Interfaces;

/// <summary>
/// Abstraction over the EF Core DbContext exposed to the Application layer.
/// Handlers read via repository interfaces; this interface exists solely for IUnitOfWork.
/// </summary>
public interface IAppDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
