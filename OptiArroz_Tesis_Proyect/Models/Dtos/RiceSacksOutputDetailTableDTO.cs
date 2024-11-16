using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Models.Dtos
{
    public class RiceSacksOutputDetailTableDTO
    {
        public int IdRiceSacksOutputDetail { get; set; }

        public int SacksQuantity { get; set; }

        public int IdRiceSacksOutput { get; set; }

        public int IdRiceLot { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Classification { get; set; } = string.Empty;

        public string Ubication { get; set; } = string.Empty;
        public DateTime ExpirationDate { get; set; }

    }
}
