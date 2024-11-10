

using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Data.Interfaces
{
    public interface IZoneDA
    {
        public Task<List<Zone>> GetActiveZones();
    }
}
