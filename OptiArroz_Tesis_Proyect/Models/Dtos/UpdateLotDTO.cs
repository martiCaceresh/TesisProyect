namespace OptiArroz_Tesis_Proyect.Models.Dtos
{
    public class UpdateLotDTO
    {
        public int IdLot { get; set; }
        public DateTime ReceptionDate { get; set; }
        public DateTime ExpirationDate { get; set; }

        public int IdClassification { get; set; }
    }
}
