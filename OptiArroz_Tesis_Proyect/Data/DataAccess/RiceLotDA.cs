using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OptiArroz_Tesis_Proyect.Data.Interfaces;
using OptiArroz_Tesis_Proyect.Models.Entities;
using OptiArroz_Tesis_Proyect.Models.Utils;
using OptiArroz_Tesis_Proyect.Services;

namespace OptiArroz_Tesis_Proyect.Data.DataAccess
{
    public class RiceLotDA : IRiceLotDA
    {
        private readonly ApplicationDbContext DbContext;
        private readonly IMapper Mapper;
        public readonly SignalRHub signalRHub;

        public RiceLotDA(SignalRHub signalRHub, IMapper Mapper, ApplicationDbContext DbContext)
        {
            this.DbContext = DbContext;
            this.Mapper = Mapper;
            this.signalRHub = signalRHub;
        }

        public async Task CreateRiceLot(RiceLot NewRiceLot)
        {
            await StockSemaphore.Semaphore.WaitAsync();
            using var transaction = await DbContext.Database.BeginTransactionAsync();
            try
            {
                var RiceLotRegister = await RegisterLot(NewRiceLot);
                await UpdateLastUbicationWithLot(RiceLotRegister);
                // Confirmar la transacción
                await transaction.CommitAsync();
                return;
            }
            catch (Exception ex)
            {
                // Revertir todos los cambios si ocurre un error
                await transaction.RollbackAsync();
                throw new Exception("Fallo al crear el lote", ex);
            }
            finally
            {
                StockSemaphore.Semaphore.Release();
            }
            
        }

        private async Task<RiceLot> RegisterLot(RiceLot NewRiceLot)
        {
            //var LastRiceLot = await DbContext.RiceLots.Where(x => x.CreatedAt.Date == DateTime.Now.Date.AddDays(1)).OrderByDescending(x => x.Code).FirstOrDefaultAsync();
            var RiceLots = await DbContext.RiceLots.ToListAsync();
            var Classification = await DbContext.RiceClassifications.FindAsync(NewRiceLot.IdClassification) ?? throw new Exception("No se encontro la clasificacion del lote");
            Classification.CurrentStock += NewRiceLot.InitialQuantity;
            var LastRiceLot = RiceLots.Where(x => x.CreatedAt.Date == DateTime.Now.Date).OrderByDescending(x => x.Code).FirstOrDefault();

            if (LastRiceLot == null)
            {
                //Crear el primer lote del dia
                NewRiceLot.Code = $"LT-{DateTime.Now:yyyyMMdd}-00001";
            }
            else
            {
                //Aumentar el correlativo del LastRiceLot
                // Obtener el número correlativo del último código
                string lastNumber = LastRiceLot.Code.Split('-')[2];
                int correlativo = int.Parse(lastNumber) + 1;

                // Crear nuevo código con correlativo aumentado
                NewRiceLot.Code = $"LT-{DateTime.Now:yyyyMMdd}-{correlativo:00000}";
            }

            await DbContext.RiceLots.AddAsync(NewRiceLot);
            
            await DbContext.SaveChangesAsync();

            var notificationType = await DbContext.NotificationTypes.Where(x => x.Name == "STOCKS").FirstOrDefaultAsync() ?? throw new Exception();

            if (Classification.CurrentStock >= Classification.MaximunStock)
            {
                //////////////////////////////////////////////////////ªªªªªªGENERAR ALERTA POR STOCK MAXIMO NO OLVIDARªªªªªªª///////////////////////////////////////////////////////
                //Notifications
                var title = "¡Se llego al stock máximo!";
                var message = "Stock maximo de " + Classification.MaximunStock + " sacos en la clasificacion " + Classification.Name + " ha sido alcanzado con un stock actual de " + Classification.CurrentStock;
                var messageType = "WARNING";
                var link = "/Home/Index";
                try
                {
                    await signalRHub.SendToRole("ADMINISTRADOR", title, message, messageType, 1, link, "");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            else if (Math.Abs(Classification.CurrentStock - Classification.MaximunStock) <= notificationType.PriorNotificationDays)
            {
                //////////////////////////////////////////////////////ªªªªªªGENERAR ALERTA POR STOCK MAXIMO NO OLVIDARªªªªªªª///////////////////////////////////////////////////////
                //Notifications
                var title = "¡Nos encontramos proximo al stock maximo!";
                var message = "Proximo al stock maximo de " + Classification.MaximunStock + " en la clasificacion " + Classification.Name + " con un stock actual de " + Classification.CurrentStock;
                var messageType = "INFO";
                var link = "/Home/Index";
                try
                {
                    await signalRHub.SendToRole("ADMINISTRADOR", title, message, messageType, 1, link, "");
                    await signalRHub.SendToRole("COLABORATOR", title, message, messageType, 1, link, "");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return NewRiceLot;
        }


        private async Task UpdateLastUbicationWithLot(RiceLot NewRiceLot)
        {
            if (NewRiceLot.IdLastUbication != null)
            {
                var Ubication = await DbContext.Ubications.FindAsync(NewRiceLot.IdLastUbication) ?? throw new Exception("Ubicacion no encontrada.");
                Ubication.IdCurrentRiceLot = NewRiceLot.IdLot;
                Ubication.State = 0; //Ocupado
                DbContext.Entry(Ubication).State = EntityState.Modified;
                await DbContext.SaveChangesAsync();
            }
            return;
        }

        public async Task<List<RiceLot>> GetActiveRiceLot()
        {
            try
            {
                var RiceLots = await DbContext.RiceLots
                .Include(x => x.RiceClassification)
                .Include(x => x.Zone)
                .Include(x => x.LastUbication)
                .ThenInclude(x => x!.Zone)
                .ToListAsync();
                return RiceLots;
            }
            catch
            {
                throw;
            }
            
        }

        public async Task<RiceLot> GetRiceLotById(int IdLot)
        {

            try
            {
                var RiceLot = await DbContext.RiceLots
                    .Include(x => x.RiceClassification)
                    .Include(x => x.Zone)
                    .Include(x=>x.CreatedBy)
                    .Include (x=>x.UpdatedBy)
                    .Include(x => x.LastUbication)
                    .ThenInclude(x => x!.Zone)
                    .Where(x => x.IdLot == IdLot)
                    .FirstOrDefaultAsync();

                return RiceLot ?? new RiceLot();
            }
            catch
            {
                throw;
            }
            
        }

        public async Task<List<RiceLot>> GetRiceLotConsultation(int IdClassification)
        {

            try
            {
                var RiceLots = await DbContext.RiceLots
                    .Include(x => x.RiceClassification)
                    .Include(x => x.Zone)
                    .Include(x => x.LastUbication)
                    .ThenInclude(x => x!.Zone)
                    .Where(x => x.IdClassification == IdClassification && x.State == 1)
                    .OrderBy(x => x.ExpirationDate)
                    .ThenBy(x => x.LeftoverQuantity)
                    .ToListAsync();

                return RiceLots ?? new List<RiceLot>();
            }
            catch
            {
                throw;
            }

        }

        public async Task<RiceLot> UpdateRiceLot(RiceLot UpdateRiceLot)
        {
            try
            {
                var FoundRiceLot = await DbContext.RiceLots.FindAsync(UpdateRiceLot.IdLot) ?? throw new Exception ("No se encontró el lote") ;
                FoundRiceLot.ExpirationDate = UpdateRiceLot.ExpirationDate;
                FoundRiceLot.ReceptionDate = UpdateRiceLot.ReceptionDate;
                FoundRiceLot.TechnicalSpecification = UpdateRiceLot.TechnicalSpecification;
                FoundRiceLot.UpdatedAt = UpdateRiceLot.UpdatedAt;
                FoundRiceLot.IdUpdatedBy = UpdateRiceLot.IdUpdatedBy;
                FoundRiceLot.State = UpdateRiceLot.State;
                DbContext.Entry(FoundRiceLot).State = EntityState.Modified;
                await DbContext.SaveChangesAsync();
                return FoundRiceLot;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<RiceLot>> GetLotsExpiringInDays(int PriorNotificationDays)
        {

            var currentDate = DateTime.Now.Date;

            var lots = await DbContext.RiceLots
                            .Where(x => x.State == 1 && x.LeftoverQuantity > 0 && MySqlDbFunctionsExtensions.DateDiffDay(EF.Functions, currentDate, x.ExpirationDate) <= PriorNotificationDays)
                            .OrderBy(x => x.ExpirationDate)
                            .ToListAsync();

            return lots;
        }
    }
}
