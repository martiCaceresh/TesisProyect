using Microsoft.EntityFrameworkCore;
using OptiArroz_Tesis_Proyect.Data.Interfaces;
using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Data.DataAccess
{
    public class ZoneDA : IZoneDA
    {
        private readonly ApplicationDbContext DbContext;

        public ZoneDA(ApplicationDbContext DbContext)
        {
            this.DbContext = DbContext;
        }
        public async Task<List<Zone>> GetActiveZones()
        {
            var Zones = await DbContext.Zones.Where(x => x.State == 1).ToListAsync();
            return Zones;
        }
    }
}
