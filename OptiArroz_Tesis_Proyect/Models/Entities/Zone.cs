using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Xml.Linq;

namespace OptiArroz_Tesis_Proyect.Models.Entities
{
    public class Zone
    {
        [Key]
        public int IdZone { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal Length { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal Width { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal Height { get; set; }

        public int Rows { get; set; }

        public int Columns { get; set; }

        public List<Ubication>? Ubications { get; set; }
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

        public Zone () { }
        public Zone (string Name , string Description, decimal Length , decimal Width, decimal Height , int Rows , int Columns ,string IdUser)
        {
            this.Name = Name.ToUpper();
            this.Width = Width;
            this.Height = Height;
            this.Length = Length;
            this.Description = Description.ToUpper();
            this.Rows = Rows;
            this.Columns = Columns;
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
