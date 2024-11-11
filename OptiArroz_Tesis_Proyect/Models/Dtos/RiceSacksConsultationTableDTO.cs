using OptiArroz_Tesis_Proyect.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace OptiArroz_Tesis_Proyect.Models.Dtos
{
    public class RiceSacksConsultationTableDTO
    {

        public string Code { get; set; } = string.Empty;

        public int LeftoverQuantity { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ExpirationDate { get; set; }

        public string Classification { get; set; } = string.Empty;


        public string LastUbication { get; set; } = string.Empty;

        public int QuantitySelected { get; set; } = 0;
    }
}
