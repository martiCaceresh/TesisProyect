﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace OptiArroz_Tesis_Proyect.Models.Entities
{
    public class Ubication
    {
        [Key]
        public int IdUbication { get; set; }

        public int Row { get; set; }
        public int Column { get; set; }

        public int IdZone { get; set; }

        public Zone? Zone { get; set; }

        [DefaultValue(1)]
        [Display(Name = "ESTADO")]
        public int State { get; set; }

        public int? IdCurrentRiceLot { get; set; }
        public List<RiceLot>? Lots { get; set; }

        public List<RiceLotMovement>? RiceLotMovementOrigins { get; set; }
        public List<RiceLotMovement>? RiceLotMovementDestinations { get; set; }

        public Ubication() { }
        public Ubication(int Row, int Column, int IdZone, int State)
        {
            this.Row = Row;
            this.Column = Column;
            this.IdZone = IdZone;
            this.State = State;
        }

        public string UbicationName ()
        {
            if (Zone == null) return "";
            return Zone.Name + " - " + "F" + Row + "C" + Column;
        }
    }
}
