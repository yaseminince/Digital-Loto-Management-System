using LOTO.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LOTO.Infrastructure.Configurations
{
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);

            builder.Property(x => x.IsActive).IsRequired();

            builder.HasMany(x => x.Lotos).WithOne(x => x.Location) .HasForeignKey(x => x.LocationId).OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
                new Location
                {
                    Id = 1,
                    Name = "Production Line 1",
                    IsActive = true,
                    CreatedBy = 1,
                    CreatedDate = new DateTime(2026, 1, 1)
                },
                new Location
                {
                    Id = 2,
                    Name = "Production Line 2",
                    IsActive = true,
                    CreatedBy = 1,
                    CreatedDate = new DateTime(2026, 1, 1)
                },
                new Location
                {
                    Id = 3,
                    Name = "Maintenance Area",
                    IsActive = true,
                    CreatedBy = 1,
                    CreatedDate = new DateTime(2026, 1, 1)
                },
                new Location
                {
                    Id = 4,
                    Name = "Warehouse",
                    IsActive = true,
                    CreatedBy = 1,
                    CreatedDate = new DateTime(2026, 1, 1)
                },
                new Location
                {
                    Id = 5,
                    Name = "Utility Room",
                    IsActive = true,
                    CreatedBy = 1,
                    CreatedDate = new DateTime(2026, 1, 1)
                }
            );
        }
    }
}