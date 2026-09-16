using LOTO.Application.DTO.Request;
using LOTO.Application.DTO.Response;
using LOTO.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOTO.Application.Interfaces
{
    public interface INotificationService : IGenericService<NotificationResponse, NotificationRequest>
    {
        Task CreateAsync(int userId,int lotoId,NotificationType type);
        Task<NotificationResponse> GetUserNotificationsAsync(int userId);

        Task MarkAsReadAsync(int notificationId, int userId);

        Task<int> GetUnreadCountAsync(int userId);

        Task CreateAdminNotificationAsync(int lotoId, NotificationType type, string ownerName);
    }
}
