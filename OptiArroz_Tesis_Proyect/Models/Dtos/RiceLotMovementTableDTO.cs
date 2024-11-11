using OptiArroz_Tesis_Proyect.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace OptiArroz_Tesis_Proyect.Models.Dtos
{
    public class RiceLotMovementTableDTO
    {

        public string Origin { get; set; } = string.Empty;

        public string Destination { get; set; } = string.Empty ;

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss tt}", ApplyFormatInEditMode = true)]
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;

    }
}
