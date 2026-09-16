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
    public class RightConfiguration : IEntityTypeConfiguration<Right>
    {
        public void Configure(EntityTypeBuilder<Right> builder)
        {
            builder.HasIndex(x => x.Name)
                   .IsUnique();

            builder.HasData(
                new Right
                {
                    Id = 1,
                    Name = "CreateLoto",
                    CreatedBy = 1,
                    CreatedDate = new DateTime(2026, 1, 1)
                },
                new Right
                {
                    Id = 2,
                    Name = "CloseOwnLoto",
                    CreatedBy = 1,
                    CreatedDate = new DateTime(2026, 1, 1)
                },
                new Right
                {
                    Id = 3,
                    Name = "ViewOwnLoto",
                    CreatedBy = 1,
                    CreatedDate = new DateTime(2026, 1, 1)
                },
                new Right
                {
                    Id = 4,
                    Name = "ViewAllLoto",
                    CreatedBy = 1,
                    CreatedDate = new DateTime(2026, 1, 1)
                },
                new Right
                {
                    Id = 5,
                    Name = "DeleteLoto",
                    CreatedBy = 1,
                    CreatedDate = new DateTime(2026, 1, 1)
                },
                new Right
                {
                    Id = 6,
                    Name = "ManageUsers",
                    CreatedBy = 1,
                    CreatedDate = new DateTime(2026, 1, 1)
                },
                new Right
                {
                    Id = 7,
                    Name = "ManageMasters",
                    CreatedBy = 1,
                    CreatedDate = new DateTime(2026, 1, 1)
                }
            );
        }
    }
}
