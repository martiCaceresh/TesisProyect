using OptiArroz_Tesis_Proyect.Models.Dtos;
using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Models.ViewModels
{
    public class CreateNewLotVM
    {
        public List<RiceClassification> RiceClassifications { get; set; } = new List<RiceClassification>();

        public List<Zone> Zones { get; set; } = new List<Zone>();

        public CreateNewLotDTO CreateNewLotDTO { get; set; } = new CreateNewLotDTO();
    }
}
