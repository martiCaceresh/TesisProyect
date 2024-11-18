namespace OptiArroz_Tesis_Proyect.Models.Dtos
{
    public class CreateNewDevolutionDTO
    {
        public int IdRiceSacksDevolutionType { get; set; }
        public string Observation { get; set; } = string.Empty;
        public string RiceSacksOutput { get; set; } = string.Empty;
        public string Classifications { get; set; } = string.Empty;
        public string ExpirationDates { get; set; } = string.Empty;
        public string QuantityDevolutions { get; set; } = string.Empty; 
    }
}
