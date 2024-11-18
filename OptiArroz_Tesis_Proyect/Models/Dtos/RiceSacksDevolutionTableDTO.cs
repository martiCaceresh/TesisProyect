using System.ComponentModel.DataAnnotations;

namespace OptiArroz_Tesis_Proyect.Models.Dtos
{
    public class RiceSacksDevolutionTableDTO
    {
        public int IdRiceSacksDevolution { get; set; }
        public string Code { get; set; } = string.Empty;

        public string RiceSacksOutput { get; set; } = string.Empty;

        public string RiceSacksDevolutionType { get; set; } = string.Empty;

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss tt}", ApplyFormatInEditMode = true)]
        public DateTime CreatedAt { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss tt}", ApplyFormatInEditMode = true)]
        public DateTime UpdatedAt { get; set; }

        public string CreatedBy { get; set; } = string.Empty;

        public string UpdatedBy { get; set; } = string.Empty;

        public string Observation { get; set; } = string.Empty;

        public List<RiceSacksDevolutionDetailTableDTO> RiceSacksDevolutionDetailTableDTOs { get; set; } = new List<RiceSacksDevolutionDetailTableDTO>();
    }
}
