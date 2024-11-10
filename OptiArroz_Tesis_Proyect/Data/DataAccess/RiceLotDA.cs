using Microsoft.EntityFrameworkCore;
using OptiArroz_Tesis_Proyect.Data.Interfaces;
using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Data.DataAccess
{
    public class RiceLotDA : IRiceLotDA
    {
        private readonly ApplicationDbContext DbContext;

        public RiceLotDA(ApplicationDbContext DbContext)
        {
            this.DbContext = DbContext;
        }

        public async Task CreateRiceLot(RiceLot NewRiceLot)
        {
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
            
        }

        private async Task<RiceLot> RegisterLot(RiceLot NewRiceLot)
        {
            var LastRiceLot = await DbContext.RiceLots.Where(x => x.CreatedAt.Date == DateTime.Now.Date).OrderByDescending(x => x.Code).FirstOrDefaultAsync();
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
    }
}
