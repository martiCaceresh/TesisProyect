using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Xml.Linq;

namespace OptiArroz_Tesis_Proyect.Models.Entities
{
    public class RiceSack
    {
        [Key]
        public int IdSack { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal Weight { get; set; }

        public List<RiceClassification>? RiceClassifications { get; set; }

        [Display(Name = "FECHA CREACION")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "FECHA MODIFICACION")]
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

        public RiceSack() { }
        public RiceSack (decimal Weight , string IdUser)
        {
            this.Weight = Weight;
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
