using LOTO.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOTO.Infrastructure.Configurations
{
    public class LotoConfiguration : IEntityTypeConfiguration<Loto>
    {
        public void Configure(EntityTypeBuilder<Loto> builder)
        {
            builder.HasIndex(x => x.LotoNumber).IsUnique();

            builder.HasOne(x => x.User).WithMany(x => x.Lotos).HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Plant) .WithMany(x => x.Lotos).HasForeignKey(x => x.PlantId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Department).WithMany(x => x.Lotos).HasForeignKey(x => x.DepartmentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Location).WithMany(x => x.Lotos).HasForeignKey(x => x.LocationId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Equipment) .WithMany(x => x.Lotos).HasForeignKey(x => x.EquipmentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.LotoType) .HasConversion<int>();

            builder.Property(x => x.Status) .HasConversion<int>();
        }
    }
}
