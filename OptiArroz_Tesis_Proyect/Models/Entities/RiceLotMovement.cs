using OptiArroz_Tesis_Proyect.Models.Dtos;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OptiArroz_Tesis_Proyect.Models.Entities
{
    public class RiceLotMovement
    {
        [Key]
        public int IdRiceLotMovement { get; set; }

        public DateTime RiceLotMovementDate { get; set; }

        public string Observation { get; set; } = string.Empty;

        public int IdRiceLot { get; set; }
        public RiceLot? RiceLot { get; set; }

        public int? IdOrigin { get;set; }
        public Ubication? Origin { get; set; }

        public int IdZoneOrigin { get; set; }
        public Zone? ZoneOrigin { get; set; }

        public int? IdDestination { get; set; }
        public Ubication? Destination { get; set; }

        public int IdZoneDestination { get; set; }
        public Zone? ZoneDestination { get; set; }

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

        public RiceLotMovement() { }

        public RiceLotMovement(CreateRiceLotMovementDTO NewLotMovement , string IdUser) 
        {
            this.RiceLotMovementDate = DateTime.Now;
            this.IdRiceLot = NewLotMovement.IdRiceLot;
            this.Observation = "";
            this.IdOrigin = NewLotMovement.IdOrigin == 0 ? null : NewLotMovement.IdOrigin; 
            this.IdZoneOrigin = NewLotMovement.IdZoneOrigin;
            this.IdDestination = NewLotMovement.IdDestination == 0 ? null : NewLotMovement.IdDestination;
            this.IdZoneDestination = NewLotMovement.IdZoneDestination;
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
