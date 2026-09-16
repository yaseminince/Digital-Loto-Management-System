using LOTO.Application.Interfaces.Repository;
using LOTO.Infrastructure.EFCore;
using LOTO.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOTO.Infrastructure.Utils
{
    public static class ServiceExtension
    {
        public static IServiceCollection DependencyInjection(this IServiceCollection services)
        {
            services.AddDbContext<LotoContext>(options =>
                options.UseSqlite("Data Source=loto.db"));

            services.AddScoped<IRightRepository, RightRepository>();
            services.AddScoped<ILotoRepository, LotoRepository>();
            services.AddScoped<IPlantRepository, PlantRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IEquipmentRepository, EquipmentRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();

            return services;
        }
    }
}
