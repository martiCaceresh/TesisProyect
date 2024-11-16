using Microsoft.EntityFrameworkCore;
using OptiArroz_Tesis_Proyect.Data.Interfaces;
using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Data.DataAccess
{
    public class RiceSacksOutputTypeDA : IRiceSacksOutputTypeDA
    {
        private readonly ApplicationDbContext DbContext;

        public RiceSacksOutputTypeDA(ApplicationDbContext DbContext)
        {
            this.DbContext = DbContext;
        }
        public async Task<List<RiceSacksOutputType>> GetRiceSacksOutputTypes()
        {
            var Types = await DbContext.RiceSacksOutputTypes.Where(x => x.State == 1).ToListAsync();
            return Types;
        }
    }
}
