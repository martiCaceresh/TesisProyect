using OptiArroz_Tesis_Proyect.Models.Dtos;
using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Models.ViewModels
{
    public class SackManagerVM
    {
        public List<RiceSack> ActiveList { get; set; } = new List<RiceSack>();
        public List<RiceSack> InactiveList { get; set; } = new List<RiceSack>();
    }
}
