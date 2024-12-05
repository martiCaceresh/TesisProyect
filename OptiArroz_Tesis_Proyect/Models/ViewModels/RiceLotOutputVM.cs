using OptiArroz_Tesis_Proyect.Models.Dtos;
using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Models.ViewModels
{
    public class RiceLotOutputVM
    {
        public RiceLotDetailDTO RiceLotDetailDTO { get; set; } = new RiceLotDetailDTO();
        public List<RiceSacksOutputType> RiceSacksOutputTypes { get; set; } = new List<RiceSacksOutputType>();
    }
}
