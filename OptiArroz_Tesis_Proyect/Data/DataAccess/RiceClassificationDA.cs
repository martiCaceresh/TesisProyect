using Microsoft.EntityFrameworkCore;
using OptiArroz_Tesis_Proyect.Data.Interfaces;
using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Data.DataAccess
{
    public class RiceClassificationDA : IRiceClassificationDA
    {
        private readonly ApplicationDbContext DbContext;

        public RiceClassificationDA(ApplicationDbContext DbContext)
        {
            this.DbContext = DbContext;
        }
        public async Task<List<RiceClassification>> GetActiveRiceClassifications()
        {
            var Classifications = await DbContext.RiceClassifications.Where(x => x.State == 1).ToListAsync();
            return Classifications;
        }
    }
}
