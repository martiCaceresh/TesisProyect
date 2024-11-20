using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OptiArroz_Tesis_Proyect.Data.Interfaces;
using OptiArroz_Tesis_Proyect.Models.Dtos;
using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Data.DataAccess
{
	public class NotificationSenderDA : INotificationSenderDA
	{
        public readonly ApplicationDbContext DbContext;
        public readonly UserManager<ApplicationUser> UserManager;
        public NotificationSenderDA(ApplicationDbContext DbContext, UserManager<ApplicationUser> UserManager)
        {
            this.DbContext = DbContext;
            this.UserManager = UserManager;
        }
        public void SendNotification(List<string> PhoneNumbers, string Message)
        {
            return;
        }

        public async Task<List<NotificationDTO>> GetNotificationsByUser(string userId)
        {
            try
            {
                var notifications = await DbContext.Notifications.Include(x => x.NotificationType).Where(x => x.IdSendTo == userId).OrderByDescending(x => x.CreatedAt).Take(20).ToListAsync();
                //convert notifications to notificationDTO
                var notificationDTOs = notifications.Select(x => new NotificationDTO
                {
                    Id = x.IdNotification,
                    SentTo = x.IdSendTo,
                    Message = x.Message,
                    MessageType = x.MessageType,
                    Type = x.NotificationType.Name,
                    Url = x.Url,
                    WasRead = x.WasRead,
                    CreatedAt = x.CreatedAt,
                    ReadAt = x.ReadAt
                }).ToList();

                return notificationDTOs;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get notifications", ex);
            }
        }

        public async Task<List<NotificationDTO>> GetAllNotificationsByUser(string userId)
        {
            try
            {
                var notifications = await DbContext.Notifications.Include(x => x.NotificationType).Where(x => x.IdSendTo == userId).OrderByDescending(x => x.CreatedAt).ToListAsync();
                //convert notifications to notificationDTO
                var notificationDTOs = notifications.Select(x => new NotificationDTO
                {
                    Id = x.IdNotification,
                    SentTo = x.IdSendTo,
                    Message = x.Message,
                    MessageType = x.MessageType,
                    Type = x.NotificationType.Name,
                    Url = x.Url,
                    WasRead = x.WasRead,
                    CreatedAt = x.CreatedAt,
                    ReadAt = x.ReadAt
                }).ToList();

                return notificationDTOs;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get notifications", ex);
            }
        }

        public async Task Insert(Notification notification)
        {
            try
            {
                await DbContext.Notifications.AddAsync(notification);
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add notification", ex);
            }
        }

        public async Task MarkAsRead(int id)
        {
            try
            {
                var notification = await DbContext.Notifications.Where(x => x.IdNotification == id).FirstOrDefaultAsync();
                notification.WasRead = 0;
                notification.ReadAt = DateTime.Now;
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to mark as read", ex);
            }
        }

        public async Task<Notification> GetLastNotificationForLot(int IdLot, int IdNotificationType)
        {
            string url = "/RiceLot/RiceLotDetails?IdLot=" + IdLot;
            return await DbContext.Notifications.Where(x => x.Url != null && x.Url == url).OrderByDescending(x => x.CreatedAt).FirstOrDefaultAsync() ?? new Notification();
        }
    }
}
