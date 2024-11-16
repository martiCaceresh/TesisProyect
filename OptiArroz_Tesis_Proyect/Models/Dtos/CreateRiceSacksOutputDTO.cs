namespace OptiArroz_Tesis_Proyect.Models.Dtos
{
    public class CreateRiceSacksOutputDTO
    {
        public int IdRiceSacksOutputType { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string Observation { get; set; } = string.Empty;

        public string Consultation { get; set; } = string.Empty;

        public IFormFile? Attachment {get; set;}

    }
}
