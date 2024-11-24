using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Data.Interfaces
{
    public interface ISackDA
    {
        public Task<List<RiceSack>> GetRiceSacks(int state);
        public Task Create(RiceSack NewRiceSack);
        public Task Activate(int Id);
        public Task Deactivate(int Id);
    }
}
