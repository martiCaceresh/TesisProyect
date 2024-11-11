using Microsoft.EntityFrameworkCore;
using OptiArroz_Tesis_Proyect.Data.Interfaces;
using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Data.DataAccess
{
    public class RiceLotMovementDA : IRiceLotMovementDA
    {
        private readonly ApplicationDbContext DbContext;

        public RiceLotMovementDA(ApplicationDbContext DbContext)
        {
            this.DbContext = DbContext;
        }
        public async Task<List<RiceLotMovement>> GetRiceLotMovementByLot(int IdLot)
        {
            var Movements = await DbContext.RiceLotMovements.Where(x=>x.IdRiceLot == IdLot)
                .Include(x=>x.Origin)
                .Include(x => x.Destination)
                .Include(x => x.ZoneDestination)
                .Include(x => x.ZoneOrigin)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
            return Movements;
        }

        public async Task CreatRiceLotMovement (RiceLotMovement NewRiceLotMovement)
        {
            using var transaction = await DbContext.Database.BeginTransactionAsync();
            try
            {
                await DbContext.RiceLotMovements.AddAsync(NewRiceLotMovement);
                await DbContext.SaveChangesAsync();

                await UpdateRiceLot(NewRiceLotMovement);
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
        }

        private async Task UpdateRiceLot (RiceLotMovement NewRiceLotMovement)
        {
            var FoundRiceLot = await DbContext.RiceLots.FindAsync(NewRiceLotMovement.IdRiceLot) ?? throw new Exception("No se encontro el lote.");

            //Establecer la ultima ubicacion del lote
            FoundRiceLot.IdLastUbication = NewRiceLotMovement.IdDestination;
            FoundRiceLot.IdZone = NewRiceLotMovement.IdZoneDestination;
            DbContext.Entry(FoundRiceLot).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();

            await UpdateLastUbicationWithLot(FoundRiceLot);

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
    }
}
