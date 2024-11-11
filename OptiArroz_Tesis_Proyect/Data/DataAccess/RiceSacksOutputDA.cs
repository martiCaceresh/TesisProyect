using Microsoft.EntityFrameworkCore;
using OptiArroz_Tesis_Proyect.Data.Interfaces;
using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Data.DataAccess
{
    public class RiceSacksOutputDA : IRiceSacksOutputDA
    {
        private readonly ApplicationDbContext DbContext;

        public RiceSacksOutputDA(ApplicationDbContext DbContext)
        {
            this.DbContext = DbContext;
        }
        public Task<List<RiceSacksOutput>> GetRiceSacksOutputs()
        {
            var Outputs = DbContext.RiceSacksOutputs
                .Include(x=>x.CreatedBy)
                .Include(x=>x.UpdatedBy)
                .Include(x=>x.RiceSacksOutputType)
                .ToListAsync();

            return Outputs;
        }

        
    }
}
