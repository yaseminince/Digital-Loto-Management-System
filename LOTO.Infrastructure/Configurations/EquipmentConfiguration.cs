using LOTO.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LOTO.Infrastructure.Configurations
{
    public class EquipmentConfiguration : IEntityTypeConfiguration<Equipment>
    {
        public void Configure(EntityTypeBuilder<Equipment> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);

            builder.Property(x => x.IsActive).IsRequired();

            builder.HasMany(x => x.Lotos).WithOne(x => x.Equipment).HasForeignKey(x => x.EquipmentId) .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
                new Equipment
                {
                    Id = 1,
                    Name = "Conveyor",
                    IsActive = true,
                    CreatedBy = 1,
                    CreatedDate = new DateTime(2026, 1, 1)
                },
                new Equipment
                {
                    Id = 2,
                    Name = "Compressor",
                    IsActive = true,
                    CreatedBy = 1,
                    CreatedDate = new DateTime(2026, 1, 1)
                },
                new Equipment
                {
                    Id = 3,
                    Name = "Hydraulic Press",
                    IsActive = true,
                    CreatedBy = 1,
                    CreatedDate = new DateTime(2026, 1, 1)
                },
                new Equipment
                {
                    Id = 4,
                    Name = "Industrial Oven",
                    IsActive = true,
                    CreatedBy = 1,
                    CreatedDate = new DateTime(2026, 1, 1)
                },
                new Equipment
                {
                    Id = 5,
                    Name = "Assembly Machine",
                    IsActive = true,
                    CreatedBy = 1,
                    CreatedDate = new DateTime(2026, 1, 1)
                }
            );
        }
    }
}