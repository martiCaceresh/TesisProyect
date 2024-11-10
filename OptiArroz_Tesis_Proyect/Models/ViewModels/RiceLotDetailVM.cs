using OptiArroz_Tesis_Proyect.Models.Dtos;
using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Models.ViewModels
{
    public class RiceLotDetailVM
    {
        public List<RiceClassification> Classifications { get; set; } = new List<RiceClassification>();
        public RiceLotDetailDTO RiceLotDetailDTO { get; set; } = new RiceLotDetailDTO();

        public UpdateLotDTO UpdateLotDTO { get; set; } = new UpdateLotDTO();
    }
}
