using Budgetly.Domain.Entities;
using Budgetly.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Budgetly.Infrastructure.Persistence.Configurations;

public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public static readonly Guid SalaryId    = Guid.Parse("a1b2c3d4-0000-0000-0000-000000000001");
    public static readonly Guid GroceriesId = Guid.Parse("a1b2c3d4-0000-0000-0000-000000000002");
    public static readonly Guid RentId      = Guid.Parse("a1b2c3d4-0000-0000-0000-000000000003");

    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.Type)
            .HasConversion<int>()
            .IsRequired();

        builder.HasData(
            new { Id = SalaryId,    Name = "Salary",    Type = CategoryType.Income  },
            new { Id = GroceriesId, Name = "Groceries", Type = CategoryType.Expense },
            new { Id = RentId,      Name = "Rent",      Type = CategoryType.Expense }
        );
    }
}
