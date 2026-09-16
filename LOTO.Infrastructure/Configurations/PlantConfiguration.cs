using LOTO.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LOTO.Infrastructure.Configurations
{
    public class PlantConfiguration : IEntityTypeConfiguration<Plant>
    {
        public void Configure(EntityTypeBuilder<Plant> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.HasMany(x => x.Lotos)
                .WithOne(x => x.Plant)
                .HasForeignKey(x => x.PlantId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
                new Plant
                {
                    Id = 1,
                    Name = "Oven",
                    IsActive = true,
                    CreatedBy = 1,
                    CreatedDate = new DateTime(2026, 1, 1)
                },
                new Plant
                {
                    Id = 2,
                    Name = "Washing Machine",
                    IsActive = true,
                    CreatedBy = 1,
                    CreatedDate = new DateTime(2026, 1, 1)
                },
                new Plant
                {
                    Id = 3,
                    Name = "Laundry",
                    IsActive = true,
                    CreatedBy = 1,
                    CreatedDate = new DateTime(2026, 1, 1)
                },
                new Plant
                {
                    Id = 4,
                    Name = "Refrigerator",
                    IsActive = true,
                    CreatedBy = 1,
                    CreatedDate = new DateTime(2026, 1, 1)
                },
                new Plant
                {
                    Id = 5,
                    Name = "Dishwasher",
                    IsActive = true,
                    CreatedBy = 1,
                    CreatedDate = new DateTime(2026, 1, 1)
                }
            );
        }
    }
}