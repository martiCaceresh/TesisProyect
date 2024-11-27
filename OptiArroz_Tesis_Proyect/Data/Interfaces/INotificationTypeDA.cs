using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Data.Interfaces
{
	public interface INotificationTypeDA
	{
		public Task<NotificationType> GetNotificationTypes(string name);
		public Task Update(int PriorNotificationDays, int FrecuencyAfterFirstNotification, int PriorNotificationSacks);
	}
}
