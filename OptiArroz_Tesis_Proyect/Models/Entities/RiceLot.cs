using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Reflection.Metadata;
using OptiArroz_Tesis_Proyect.Models.Dtos;

namespace OptiArroz_Tesis_Proyect.Models.Entities
{
    public class RiceLot
    {
        [Key]
        public int IdLot { get; set; }

        public string Code { get; set; } = string.Empty;

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ReceptionDate { get; set; }

        public int InitialQuantity { get; set; }

        public int LeftoverQuantity { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ExpirationDate { get; set; }

        public byte[]? TechnicalSpecification { get; set; }

        public int IdClassification { get; set; }
        public RiceClassification? RiceClassification { get; set; }


        public int? IdLastUbication { get; set; }

        public Ubication? LastUbication { get; set; }

        public int IdZone { get; set; }
        public Zone? Zone { get; set; } 

        public List<RiceLotMovement>? RiceLotMovements { get; set; }
        public List<RiceSacksDevolutionDetail>? RiceSacksDevolutionDetails { get; set; }
        public List<RiceSacksOutputDetail>? RiceSacksOutputDetails { get; set; }


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

        public RiceLot () { }

        public RiceLot (CreateNewLotDTO NewLot , byte[] TechnicalSpecification , string IdUser) 
        {
            ReceptionDate = DateTime.Now;
            InitialQuantity = NewLot.QuantitySacks;
            LeftoverQuantity = NewLot.QuantitySacks;
            ExpirationDate = NewLot.ExpirationDate;
            this.TechnicalSpecification = TechnicalSpecification;
            IdClassification = NewLot.IdClassification;
            IdLastUbication = NewLot.IdUbication == 0 ? null : NewLot.IdUbication;
            IdZone = NewLot.IdZone;
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
