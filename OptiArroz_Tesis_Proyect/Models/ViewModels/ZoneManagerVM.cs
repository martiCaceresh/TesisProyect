using OptiArroz_Tesis_Proyect.Models.Dtos;
using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Models.ViewModels
{
    public class ZoneManagerVM
    {


        public SystemConfiguration SystemConfiguration { get; set; } = new SystemConfiguration();
        public List<Zone> ActiveList { get; set; } = new List<Zone>();
        public List<Zone> InactiveList { get; set; } = new List<Zone>();
    }
}
