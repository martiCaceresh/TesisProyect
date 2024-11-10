using System.ComponentModel.DataAnnotations;

namespace OptiArroz_Tesis_Proyect.Models.Entities
{
    public class RiceSacksOutputDetail
    {
        [Key]
        public int IdRiceSacksOutputDetail { get; set; }

        public int SacksQuantity { get; set; }

        public int IdRiceSacksOutput { get; set; }
        public RiceSacksOutput? RiceSacksOutput { get; set; }

        public int IdRiceLot { get; set; }

        public RiceLot? RiceLot { get; set; }
    }
}
