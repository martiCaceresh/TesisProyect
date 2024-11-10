using Microsoft.EntityFrameworkCore;
using OptiArroz_Tesis_Proyect.Data.Interfaces;
using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Data.DataAccess
{
    public class UbicationDA : IUbicationDA
    {
        private readonly ApplicationDbContext DbContext;

        public UbicationDA(ApplicationDbContext DbContext)
        {
            this.DbContext = DbContext;
        }
        public async Task<List<Ubication>> GetUbicationsByZone(int IdZone)
        {
            var Ubications = await DbContext.Ubications.Where(x => x.IdZone == IdZone).ToListAsync();
            return Ubications?? new List<Ubication>();
        }
    }
}
