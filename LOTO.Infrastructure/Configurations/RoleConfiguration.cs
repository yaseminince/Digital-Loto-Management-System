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
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasIndex(x => x.Name).IsUnique();

            builder.HasMany(x => x.Rights).WithMany(x => x.Roles).UsingEntity<Dictionary<string, object>>(
                       "RoleRight",right => right.HasOne<Right>().WithMany().HasForeignKey("RightId")
                           .OnDelete(DeleteBehavior.Cascade),
                       role => role.HasOne<Role>().WithMany() .HasForeignKey("RoleId").OnDelete(DeleteBehavior.Cascade),
                       join =>
                       {
                           join.HasKey("RoleId", "RightId");
                           join.HasData(
                               // Admin
                               new { RoleId = 1, RightId = 1 },
                               new { RoleId = 1, RightId = 2 },
                               new { RoleId = 1, RightId = 3 },
                               new { RoleId = 1, RightId = 4 },
                               new { RoleId = 1, RightId = 5 },
                               new { RoleId = 1, RightId = 6 },
                               new { RoleId = 1, RightId = 7 },

                               // User
                               new { RoleId = 2, RightId = 1 }, // createloto
                               new { RoleId = 2, RightId = 2 }, // closeownloto
                               new { RoleId = 2, RightId = 3 } // viewownloto
                           );
                       });

            builder.HasData(
                new Role
                {
                    Id = 1,
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    CreatedBy = 1,
                    CreatedDate = new DateTime(2026, 1, 1)
                },
                new Role
                {
                    Id = 2,
                    Name = "User",
                    NormalizedName = "USER",
                    CreatedBy = 1,
                    CreatedDate = new DateTime(2026, 1, 1)
                }
            );
        }
    }
}
