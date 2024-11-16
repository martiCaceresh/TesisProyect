using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Data.Interfaces
{
    public interface IRiceSacksOutputTypeDA
    {
        public Task<List<RiceSacksOutputType>> GetRiceSacksOutputTypes();
    }
}
