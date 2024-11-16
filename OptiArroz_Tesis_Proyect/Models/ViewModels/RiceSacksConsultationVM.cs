using OptiArroz_Tesis_Proyect.Models.Dtos;
using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Models.ViewModels
{
    public class RiceSacksConsultationVM
    {
        public List<RiceSacksConsultationTableDTO> RiceSacksConsultationTableDTOs { get; set; } = new List<RiceSacksConsultationTableDTO>();
        public List<RiceClassification> RiceClassifications { get; set; } = new List<RiceClassification>();
        public List<RiceSacksOutputType> RiceSacksOutputTypes { get; set; } = new List<RiceSacksOutputType>();
        public int QuantitySelected { get; set; } = 0;
    }
}
