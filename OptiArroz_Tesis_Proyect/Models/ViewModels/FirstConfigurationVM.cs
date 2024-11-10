using OptiArroz_Tesis_Proyect.Models.Dtos;
using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Models.ViewModels
{
    public class FirstConfigurationVM
    {
        public List<RiceGrade> RiceGrades { get; set; } = new List<RiceGrade>();
        public List<RiceClass> RiceClasses { get; set; } = new List<RiceClass>();
        public CreateFirstConfigurationDTO CreateFirstConfigurationDTO { get; set; } = new CreateFirstConfigurationDTO();

        public FirstConfigurationVM()
        {

        }

    }
}
