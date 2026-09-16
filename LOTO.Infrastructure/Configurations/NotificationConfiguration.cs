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
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasOne(x => x.User)
                   .WithMany(x => x.Notifications)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Loto)
                   .WithMany(x => x.Notifications)
                   .HasForeignKey(x => x.LotoId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.Property(x => x.Type)
                   .HasConversion<int>();
        }
    }
}
