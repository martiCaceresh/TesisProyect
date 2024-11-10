using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Data.Interfaces
{
    public interface IFirstConfigurationDA
    {
        Task<bool> ValidateFirstConfiguration();
        Task<bool> CreateFirstConfiguration(List<RiceSack> Sacks, List<RiceClassification> RiceClassifications, List<Zone> Warehouses, List<Zone> OtherZones , SystemConfiguration FirstConfiguration);
    }
}
