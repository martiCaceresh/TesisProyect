using OptiArroz_Tesis_Proyect.Models.Dtos;
using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Data.Interfaces
{
    public interface IRiceLotDA
    {
        public Task<List<RiceLot>> GetActiveRiceLot();
        public Task<List<RiceLot>> GetLotsExpiringInDays(int PriorNotificationDays);
        public Task<RiceLot> GetRiceLotById(int IdLot);

        public Task CreateRiceLot(RiceLot NewRiceLot);

        public Task<RiceLot> UpdateRiceLot(RiceLot UpdateRiceLot);

        public Task<List<RiceLot>> GetRiceLotConsultation(int IdClassification);


    }
}
