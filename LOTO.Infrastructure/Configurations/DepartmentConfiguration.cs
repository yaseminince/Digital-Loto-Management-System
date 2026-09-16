using LOTO.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LOTO.Infrastructure.Configurations
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);

            builder.Property(x => x.IsActive).IsRequired();

            builder.HasMany(x => x.Lotos).WithOne(x => x.Department).HasForeignKey(x => x.DepartmentId).OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
                new Department
                {
                    Id = 1,
                    Name = "Production",
                    IsActive = true,
                    CreatedBy = 1,
                    CreatedDate = new DateTime(2026, 1, 1)
                },
                new Department
                {
                    Id = 2,
                    Name = "Maintenance",
                    IsActive = true,
                    CreatedBy = 1,
                    CreatedDate = new DateTime(2026, 1, 1)
                },
                new Department
                {
                    Id = 3,
                    Name = "Quality",
                    IsActive = true,
                    CreatedBy = 1,
                    CreatedDate = new DateTime(2026, 1, 1)
                },
                new Department
                {
                    Id = 4,
                    Name = "Engineering",
                    IsActive = true,
                    CreatedBy = 1,
                    CreatedDate = new DateTime(2026, 1, 1)
                },
                new Department
                {
                    Id = 5,
                    Name = "Warehouse",
                    IsActive = true,
                    CreatedBy = 1,
                    CreatedDate = new DateTime(2026, 1, 1)
                }
            );
        }
    }
}