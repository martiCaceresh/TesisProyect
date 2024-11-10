using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Data.Interfaces
{
    public interface IRiceClassificationDA
    {
        public Task<List<RiceClassification>> GetActiveRiceClassifications();
    }
}
