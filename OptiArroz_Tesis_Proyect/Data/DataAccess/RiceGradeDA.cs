using Microsoft.EntityFrameworkCore;
using OptiArroz_Tesis_Proyect.Data.Interfaces;
using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Data.DataAccess
{
    public class RiceGradeDA : IRiceGradeDA
    {
        private readonly ApplicationDbContext DbContext;

        public RiceGradeDA(ApplicationDbContext DbContext)
        {
            this.DbContext = DbContext;
        }
        public async Task<IEnumerable<RiceGrade>> GetActiveRiceGradesAsync()
        {
            if (DbContext.RiceGrades == null)
            {
                throw new Exception("No inicializado la tabla RiceGrades");
            }
            return await DbContext.RiceGrades
            .Where(rg => rg.State != 0)
            .ToListAsync();
        }
    }
}
