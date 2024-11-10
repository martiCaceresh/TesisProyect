using OptiArroz_Tesis_Proyect.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Xml.Linq;

namespace OptiArroz_Tesis_Proyect.Models.Dtos
{
    public class RiceLotDetailDTO
    {
        public int IdLot { get; set; }

        public string Code { get; set; } = string.Empty;

        public DateTime ReceptionDate { get; set; }

        public int InitialQuantity { get; set; }

        public int LeftoverQuantity { get; set; }

        public DateTime ExpirationDate { get; set; }

        public int IdClassification { get; set; }
        public string Classification { get; set; } = string.Empty;

        public string LastUbication { get; set; } = string.Empty;

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss tt}", ApplyFormatInEditMode = true)]
        public DateTime CreatedAt { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss tt}", ApplyFormatInEditMode = true)]
        public DateTime UpdatedAt { get; set; }

        public string State { get; set; } = string.Empty;

        public string CreatedBy { get; set; } = string.Empty;

        public string UpdatedBy { get; set; } = string.Empty;
    }
}
