using System.ComponentModel.DataAnnotations;

namespace OptiArroz_Tesis_Proyect.Models.Entities
{
    public class RiceSacksDevolutionDetail
    {
        [Key]
        public int IdRiceSacksDevolutionDetail { get; set; }

        public int SacksQuantity { get; set; }

        public int IdRiceSacksDevolution { get; set; }
        public RiceSacksDevolution? RiceSacksDevolution { get; set; }

        public int IdRiceLot { get; set; }

        public RiceLot? RiceLot { get; set; }
    }
}
