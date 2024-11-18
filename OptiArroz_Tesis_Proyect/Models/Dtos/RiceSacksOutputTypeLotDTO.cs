using System.ComponentModel.DataAnnotations;

namespace OptiArroz_Tesis_Proyect.Models.Dtos
{
    public class RiceSacksOutputTypeLotDTO
    {
        public int IdClassification { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ExpirationDate { get; set; }

        public string Classification { get; set; } = string.Empty;

        public int QuantitySelected { get; set; }
    }
}
