using OptiArroz_Tesis_Proyect.Models.Dtos;
using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Models.ViewModels
{
    public class RiceLotMovementVM
    {
        public RiceLotDetailDTO RiceLotDetailDTO { get; set; } = new RiceLotDetailDTO();
        public List<Zone> Zones { get; set; } = new List<Zone>();

        public List<RiceLotMovementTableDTO> RiceLotMovementTableDTO { get; set; } = new List<RiceLotMovementTableDTO>();

        public CreateRiceLotMovementDTO CreateRiceLotMovementDTO { get; set; } = new CreateRiceLotMovementDTO();
    }
}
