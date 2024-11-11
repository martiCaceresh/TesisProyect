using OptiArroz_Tesis_Proyect.Models.Dtos;
using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Models.ViewModels
{
    public class RiceSacksOutputVM
    {
        public List<RiceSacksOutputTableDTO> RiceSacksOutputTableDTOs { get; set; } = new List<RiceSacksOutputTableDTO>();
    }
}
