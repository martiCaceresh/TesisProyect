﻿using OptiArroz_Tesis_Proyect.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace OptiArroz_Tesis_Proyect.Models.Dtos
{
    public class CreateRiceLotMovementDTO
    {

        public int IdRiceLot { get; set; }

        public int IdZoneOrigin { get; set; }

        public int IdOrigin { get; set; }

        public int IdZoneDestination { get; set; }
        public int IdDestination { get; set; }
    }
}
