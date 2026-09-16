using LOTO.Application.Interfaces.Repository;
using LOTO.Domain.Entities;
using LOTO.Infrastructure.EFCore;
using LOTO.Infrastructure.Repository.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOTO.Infrastructure.Repository
{
    public class NotificationRepository : EFRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(LotoContext context) : base(context)
        {
        }

        public async Task<List<Notification>> GetUserNotificationsWithLotoAsync(int userId)
        {
            return await _context.Notification
                .Include(x => x.Loto)
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }
    }
}
