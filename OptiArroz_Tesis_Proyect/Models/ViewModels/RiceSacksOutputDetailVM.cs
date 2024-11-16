using OptiArroz_Tesis_Proyect.Models.Dtos;
using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Models.ViewModels
{
    public class RiceSacksOutputDetailVM
    {
        public List<RiceLotTableDTO> RiceLots { get; set; } = new List<RiceLotTableDTO> { };

    }
}
