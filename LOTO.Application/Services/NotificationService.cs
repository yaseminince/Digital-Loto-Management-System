using LOTO.Application.DTO.Request;
using LOTO.Application.DTO.Response;
using LOTO.Application.Interfaces;
using LOTO.Application.Interfaces.Repository;
using LOTO.Domain.Entities;
using LOTO.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOTO.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repository;
        private readonly UserManager<User> _userManager;

        public NotificationService(
            INotificationRepository repository,
            UserManager<User> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public NotificationResponse Add(NotificationRequest request)
        {
            var response = new NotificationResponse
            {
                Entity = new Notification(),
                Error = new DTO.Base.Error()
            };

            try
            {
                request.Entity.CreatedDate = DateTime.Now;

                response.Entity = _repository.Add(request.Entity);
            }
            catch (Exception ex)
            {
                response.Error.HasException = true;
                response.Error.Message = ex.Message;
            }

            return response;
        }

        public async Task<NotificationResponse> AddAsync(NotificationRequest request)
        {
            var response = new NotificationResponse
            {
                Entity = new Notification(),
                Error = new DTO.Base.Error()
            };

            try
            {
                request.Entity.CreatedDate = DateTime.Now;

                response.Entity = await _repository.AddAsync(request.Entity);
            }
            catch (Exception ex)
            {
                response.Error.HasException = true;
                response.Error.Message = ex.Message;
            }

            return response;
        }


        // LOTOLARDAN BILDIRIM OLUSTURMS
        public async Task CreateAsync(int userId,int lotoId,NotificationType type)
        {
            string message = type switch
            {
                NotificationType.LotoCreated =>
                    "LOTO created successfully.",

                NotificationType.LotoClosed =>
                    "LOTO closed successfully.",

                NotificationType.LotoOverdue =>
                    "Your LOTO is overdue.",

                _ => "New notification."
            };

            var notification = new Notification
            {
                UserId = userId, // kime ait
                LotoId = lotoId, // hangi loto
                Type = type, // ne oldu?
                Message = message, // type'a göre mesaj belirleniyor
                IsRead = false,
                CreatedBy = userId,
                CreatedDate = DateTime.Now
            };

            await _repository.AddAsync(notification);
        }

        public async Task CreateAdminNotificationAsync(
    int lotoId,
    NotificationType type,
    string ownerName)
        {
            var admins = await _userManager.GetUsersInRoleAsync("Admin");

            string message = type switch
            {
                NotificationType.LotoCreated =>
                    $"New LOTO created: {ownerName}.",

                NotificationType.LotoOverdue =>
                    $"LOTO is overdue. Owner: {ownerName}.",

                _ => "New LOTO notification."
            };

            foreach (var admin in admins)
            {
                var notification = new Notification
                {
                    UserId = admin.Id,
                    LotoId = lotoId,
                    Type = type,
                    Message = message,
                    IsRead = false,
                    CreatedBy = admin.Id,
                    CreatedDate = DateTime.Now
                };

                await _repository.AddAsync(notification);
            }
        }
        public NotificationResponse GetById(int id)
        {
            var response = new NotificationResponse
            {
                Entity = new Notification(),
                Error = new DTO.Base.Error()
            };

            try
            {
                response.Entity = _repository.Get(x => x.Id == id);
            }
            catch (Exception ex)
            {
                response.Error.HasException = true;
                response.Error.Message = ex.Message;
            }

            return response;
        }

        public NotificationResponse GetByFilter(NotificationRequest request)
        {
            var response = new NotificationResponse
            {
                Entity = new Notification(),
                Error = new DTO.Base.Error()
            };

            try
            {
                response.Entity = _repository.Get(x =>x.Id == request.Entity.Id);
            }
            catch (Exception ex)
            {
                response.Error.HasException = true;
                response.Error.Message = ex.Message;
            }

            return response;
        }

        public NotificationResponse GetList()
        {
            var response = new NotificationResponse
            {
                EntityList = new List<Notification>(),
                Error = new DTO.Base.Error()
            };

            try
            {
                response.EntityList = _repository.GetList();
            }
            catch (Exception ex)
            {
                response.Error.HasException = true;
                response.Error.Message = ex.Message;
            }

            return response;
        }

        public async Task<NotificationResponse> GetUserNotificationsAsync(int userId)
        {
            var response = new NotificationResponse
            {
                EntityList = new List<Notification>(),
                Error = new DTO.Base.Error()
            };

            try
            {
                response.EntityList =
                    await _repository.GetUserNotificationsWithLotoAsync(userId);
            }
            catch (Exception ex)
            {
                response.Error.HasException = true;
                response.Error.Message = ex.Message;
            }

            return response;
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            var notifications = await _repository.GetListAsync(
                x => x.UserId == userId && !x.IsRead);

            return notifications.Count;
        }

        public async Task MarkAsReadAsync(int notificationId, int userId)
        {
            var notification = _repository.Get(
                x => x.Id == notificationId &&
                     x.UserId == userId);

            if (notification == null)
                return;

            notification.IsRead = true;
            notification.UpdatedDate = DateTime.Now;
            notification.UpdatedBy = userId;

            await _repository.UpdateAsync(notification);
        }
        public NotificationResponse Update(NotificationRequest request)
        {
            var response = new NotificationResponse
            {
                Entity = new Notification(),
                Error = new DTO.Base.Error()
            };

            try
            {
                request.Entity.UpdatedDate = DateTime.Now;

                response.Entity = _repository.Update(request.Entity);
            }
            catch (Exception ex)
            {
                response.Error.HasException = true;
                response.Error.Message = ex.Message;
            }

            return response;
        }

        public async Task<NotificationResponse> UpdateAsync(NotificationRequest request)
        {
            var response = new NotificationResponse
            {
                Entity = new Notification(),
                Error = new DTO.Base.Error()
            };

            try
            {
                request.Entity.UpdatedDate = DateTime.Now;

                response.Entity = await _repository.UpdateAsync(request.Entity);
            }
            catch (Exception ex)
            {
                response.Error.HasException = true;
                response.Error.Message = ex.Message;
            }

            return response;
        }

        public void Delete(NotificationRequest request)
        {
            _repository.Delete(request.Entity);
        }

        public async Task DeleteAsync(NotificationRequest request)
        {
            await _repository.DeleteAsync(request.Entity);
        }
    }
}
