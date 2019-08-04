using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TDD.Domain.Library.Entities;

namespace TDD.Data.Library.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(x => x.EmployeeId);

            builder.Property(p => p.EmployeeId).ValueGeneratedOnAdd();

            builder.Property(x => x.FirstName).HasMaxLength(30);

            builder.Property(x => x.LastName).HasMaxLength(30);

            builder.Property(x => x.PhoneNumber).HasMaxLength(30);

            builder.Property(x => x.Email).HasMaxLength(50);
        }
    }
}
