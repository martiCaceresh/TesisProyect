

using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Data.Interfaces
{
    public interface IZoneDA
    {
        public Task<List<Zone>> GetActiveZones();
        public Task<List<Zone>> GetInactiveZones();
        public Task Create(Zone NewZone);
        public Task Update(Zone UpdateZone);

        public Task Activate(int Id);
        public Task Deactivate(int Id);

    }
}
