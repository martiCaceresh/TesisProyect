﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Xml.Linq;

namespace OptiArroz_Tesis_Proyect.Models.Entities
{
    public class RiceSacksDevolution
    {
        [Key]
        public int IdRiceSacksDevolution { get; set; }

        public string Code { get; set; } = string.Empty;

        public DateTime RiceSacksDevolutionDate { get; set; }

        public string Observation { get; set; } = string.Empty;

        public int IdRiceSacksDevolutionType { get; set; }
        public RiceSacksDevolutionType? RiceSacksDevolutionType { get; set; }

        public List<RiceSacksDevolutionDetail>? RiceSacksDevolutionDetails { get; set; }

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
