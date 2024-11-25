using OptiArroz_Tesis_Proyect.Models.Dtos;
using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Models.ViewModels
{
    public class ClassificationManagerVM
    {
        public List<RiceGrade> RiceGrades { get; set; } = new List<RiceGrade>();
        public List<RiceClass> RiceClasses { get; set; } = new List<RiceClass>();
        public List<RiceSack> RiceSacks { get; set; } = new List<RiceSack>();

        public List<RiceClassification> ActiveList { get; set; } = new List<RiceClassification>();
        public List<RiceClassification> InactiveList { get; set; } = new List<RiceClassification>();
    }
}
