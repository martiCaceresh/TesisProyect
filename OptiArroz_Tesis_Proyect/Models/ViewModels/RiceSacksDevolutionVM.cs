using OptiArroz_Tesis_Proyect.Models.Dtos;
using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Models.ViewModels
{
    public class RiceSacksDevolutionVM
    {
        public List<RiceSacksDevolutionTableDTO> RiceSacksDevolutionTableDTOs { get; set; } = new List<RiceSacksDevolutionTableDTO>();
        public List<RiceSacksDevolutionType> RiceSacksDevolutionTypes { get; set; } = new List<RiceSacksDevolutionType>();
    }
}
