﻿using OptiArroz_Tesis_Proyect.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace OptiArroz_Tesis_Proyect.Models.Dtos
{
	public class RiceSacksOutputTableDTO
	{
        public int IdRiceSacksOutput { get; set; }
        public string Code { get; set; } = string.Empty;

        public string RiceSacksOutputType { get; set; } = string.Empty;

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss tt}", ApplyFormatInEditMode = true)]
        public DateTime CreatedAt { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss tt}", ApplyFormatInEditMode = true)]
        public DateTime UpdatedAt { get; set; }

        public string CreatedBy { get; set; } = string.Empty;

        public string UpdatedBy { get; set; } = string.Empty;
    }
}