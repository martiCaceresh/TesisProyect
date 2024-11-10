using Microsoft.EntityFrameworkCore;
using OptiArroz_Tesis_Proyect.Data.Interfaces;
using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Data.DataAccess
{
    public class RiceClassDA : IRiceClassDA
    {
        private readonly ApplicationDbContext DbContext;

        public RiceClassDA(ApplicationDbContext DbContext)
        {
            this.DbContext = DbContext;
        }
        public async Task<IEnumerable<RiceClass>> GetActiveRiceClassesAsync()
        {
            if (DbContext.RiceClasses == null)
            {
                throw new Exception("No inicializado la tabla RiceGrades");
            }
            return await DbContext.RiceClasses
            .Where(rg => rg.State != 0)
            .ToListAsync();
        }
    }
}
