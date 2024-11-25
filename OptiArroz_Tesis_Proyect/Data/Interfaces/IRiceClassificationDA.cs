using OptiArroz_Tesis_Proyect.Models.Dtos;
using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Data.Interfaces
{
    public interface IRiceClassificationDA
    {
        public Task<List<RiceClassification>> GetActiveRiceClassifications();
        public Task<List<RiceClassification>> GetInactiveRiceClassifications();
        public Task Create(RiceClassification NewRiceClassification);
        public Task Update(int Id, int MinimunStock, int MaximunStock, int QuatityPerLot , string IdUser);
        public Task<List<RiceSacksConsultationResultDTO>> GetStockConsultation(List<RiceSacksConsultationDTO> consultations);

        public Task Activate(int Id);
        public Task Deactivate(int Id);
    }
}
