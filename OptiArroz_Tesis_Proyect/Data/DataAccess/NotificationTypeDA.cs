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

        public async Task Update(int PriorNotificationDays, int FrecuencyAfterFirstNotification, int PriorNotificationSacks)
        {
            try
            {
                var Stock = await DbContext.NotificationTypes.Where(x => x.Name == "STOCKS").FirstOrDefaultAsync() ?? throw new Exception("No se encontro la configuracion de stocks");
                var Expiration = await DbContext.NotificationTypes.Where(x => x.Name == "FECHA VENCIMIENTO").FirstOrDefaultAsync() ?? throw new Exception("No se encontro la configuracion de vencimientos");

                Stock.PriorNotificationDays = PriorNotificationSacks;
                Expiration.PriorNotificationDays = PriorNotificationDays;
                Expiration.FrecuencyAfterFirstNotification = FrecuencyAfterFirstNotification;

                DbContext.Entry(Stock).State = EntityState.Modified;
                DbContext.Entry(Expiration).State = EntityState.Modified;
                await DbContext.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}
