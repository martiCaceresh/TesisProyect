using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Xml.Linq;

namespace OptiArroz_Tesis_Proyect.Models.Entities
{
    public class RiceClassification
    {
        [Key]
        public int IdClassification { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int MinimunStock { get; set; }

        [Required]
        public int MaximunStock { get; set; }

        [Required]
        public int SacksPerLot { get; set; }


        public int IdRiceClass { get; set; }

        public RiceClass? RiceClass { get; set; }

        public int IdRiceGrade { get; set; }

        public RiceGrade? RiceGrade { get; set; }

        public int IdRiceSack { get; set; }
        public RiceSack? RiceSack { get; set; }

        public List<RiceLot>? RiceLots { get; set; }

        [Display(Name = "FECHA CREACION")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "FECHA MODIFICACION")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime UpdatedAt { get; set; }

        [DefaultValue(1)]
        [Display(Name = "ESTADO")]
        public int State { get; set; }

        [Display(Name = "CREADO POR")]
        public string? IdCreatedBy { get; set; }

        public ApplicationUser? CreatedBy { get; set; }

        [Display(Name = "MODIFICADO POR")]
        public string? IdUpdatedBy { get; set; }
        public ApplicationUser? UpdatedBy { get; set; }

        public RiceClassification() { }

        public RiceClassification(int MinimunStock,int MaximunStock,int SacksPerLot, int IdRiceClass , int IdRiceGrade, string RiceSack, string IdUser)
        {
            this.MinimunStock = MinimunStock;
            this.MaximunStock = MaximunStock;
            this.SacksPerLot = SacksPerLot;
            this.IdRiceClass = IdRiceClass;
            this.IdRiceGrade = IdRiceGrade;
            this.Name = RiceSack;
            SetBaseModel(IdUser, IdUser, DateTime.Now, DateTime.Now, 1);
        }
        public void SetBaseModel(string? IdCreatedBy, string? IdUpdatedBy, DateTime CreatedAt, DateTime UpdatedAt, int State)
        {
            this.IdCreatedBy = IdCreatedBy;
            this.IdUpdatedBy = IdUpdatedBy;
            this.CreatedAt = CreatedAt;
            this.UpdatedAt = UpdatedAt;
            this.State = State;
        }

        public void SetBaseModel(string? IdUpdatedBy, DateTime UpdatedAt, int State)
        {
            this.IdUpdatedBy = IdUpdatedBy;
            this.UpdatedAt = UpdatedAt;
            this.State = State;
        }
    }
}
