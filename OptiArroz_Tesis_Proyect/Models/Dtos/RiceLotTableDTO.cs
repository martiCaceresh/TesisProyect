using OptiArroz_Tesis_Proyect.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace OptiArroz_Tesis_Proyect.Models.Dtos
{
    public class RiceLotTableDTO
    {
        public int IdLot { get; set; }
        public string Code { get; set; } = string.Empty;

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ReceptionDate { get; set; }

        public int LeftoverQuantity { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ExpirationDate { get; set; }

        public string Classification { get; set; } = string.Empty;


        public string LastUbication { get; set; } = string.Empty;


    }
}
