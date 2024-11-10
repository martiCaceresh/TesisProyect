using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Data.Interfaces
{
    public interface IRiceGradeDA
    {
        Task<IEnumerable<RiceGrade>> GetActiveRiceGradesAsync();
    }
}
