using Microsoft.EntityFrameworkCore;
using OptiArroz_Tesis_Proyect.Data.Interfaces;
using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Data.DataAccess
{
	public class NotificationTypeDA : INotificationTypeDA
	{
        private readonly ApplicationDbContext DbContext;

        public NotificationTypeDA(ApplicationDbContext DbContext)
        {
            this.DbContext = DbContext;
        }
        public async Task<NotificationType> GetNotificationTypes(string name)
		{
            return await DbContext.NotificationTypes.Where(x => x.Name == name).FirstOrDefaultAsync() ?? throw new Exception("No se encontro el tipo de notificacion");
		}
	}
}
