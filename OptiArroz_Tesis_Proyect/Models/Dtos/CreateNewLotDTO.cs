namespace OptiArroz_Tesis_Proyect.Models.Dtos
{
    public class CreateNewLotDTO
    {
        public int IdClassification { get; set; }

        public int QuantitySacks { get; set; }
        public DateTime ExpirationDate { get; set; }

        public int IdUbication { get; set; }

        public int IdZone { get; set; }
    }
}
