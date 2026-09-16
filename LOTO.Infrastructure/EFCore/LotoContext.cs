using LOTO.Domain.Entities;
using LOTO.Infrastructure.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LOTO.Infrastructure.EFCore
{
    public class LotoContext : IdentityDbContext<User, Role, int>
    {
        public LotoContext(DbContextOptions<LotoContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new RightConfiguration());
            modelBuilder.ApplyConfiguration(new PlantConfiguration());
            modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
            modelBuilder.ApplyConfiguration(new LocationConfiguration());
            modelBuilder.ApplyConfiguration(new EquipmentConfiguration());
            modelBuilder.ApplyConfiguration(new LotoConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationConfiguration());
        }
        public DbSet<Right> Right { get; set; }
        public DbSet<Plant> Plant { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<Loto> Loto { get; set; }
        public DbSet<Notification> Notification { get; set; }
    }
}
