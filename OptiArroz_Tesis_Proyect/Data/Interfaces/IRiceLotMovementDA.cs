using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Data.Interfaces
{
    public interface IRiceLotMovementDA
    {
        public Task<List<RiceLotMovement>> GetRiceLotMovementByLot(int IdLot);
        public Task CreatRiceLotMovement(RiceLotMovement NewRiceLotMovement);
    }
}
