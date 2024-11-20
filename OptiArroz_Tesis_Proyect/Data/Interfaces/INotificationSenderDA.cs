using OptiArroz_Tesis_Proyect.Data.DataAccess;
using OptiArroz_Tesis_Proyect.Models.Dtos;
using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Data.Interfaces
{
	public interface INotificationSenderDA
	{
        void SendNotification(List<string> PhoneNumbers, string Message);
        Task<List<NotificationDTO>> GetNotificationsByUser(string userId);
        Task<List<NotificationDTO>> GetAllNotificationsByUser(string userId);
        Task Insert(Notification notification);
        Task MarkAsRead(int id);

        Task<Notification> GetLastNotificationForLot(int IdLot, int IdNotificationType);
    }
}
