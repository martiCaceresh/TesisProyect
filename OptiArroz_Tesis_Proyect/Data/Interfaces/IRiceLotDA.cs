using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Data.Interfaces
{
    public interface IRiceLotDA
    {
        public Task<List<RiceLot>> GetActiveRiceLot();
        public Task<RiceLot> GetRiceLotById(int IdLot);

        public Task CreateRiceLot(RiceLot NewRiceLot);
    }
}
