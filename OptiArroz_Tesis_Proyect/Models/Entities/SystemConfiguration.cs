using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Xml.Linq;
using OptiArroz_Tesis_Proyect.Models.Dtos;

namespace OptiArroz_Tesis_Proyect.Models.Entities
{
    public class SystemConfiguration
    {
        [Key]
        public int IdSystemConfiguration { get; set; }

        //SACKS
        public string Sacks { get; set; } = string.Empty;

        //CLASSIFICATION

        public string IdClasses { get; set; } = string.Empty;
        public string IdGrades { get; set; } = string.Empty;
        public string SacksWeight { get; set; } = string.Empty;
        public string MinimunStocks { get; set; } = string.Empty;
        public string MaximunStocks { get; set; } = string.Empty;
        public string QuatitiesSacksPerLot { get; set; } = string.Empty;

        //WAREHOUSE
        public string WarehouseNames { get; set; } = string.Empty;
        public string WarehouseDescription { get; set; } = string.Empty;
        public string WarehouseLength { get; set; } = string.Empty;
        public string WarehouseWidth { get; set; } = string.Empty;
        public string WarehouseHeight { get; set; } = string.Empty;
        public string WarehouseRows { get; set; } = string.Empty;
        public string WarehouseColumns { get; set; } = string.Empty;

        //OTHER ZONE

        public string OtherZoneNames { get; set; } = string.Empty;
        public string OtherZoneDescription { get; set; } = string.Empty;
        public string OtherZoneLength { get; set; } = string.Empty;
        public string OtherZoneWidth { get; set; } = string.Empty;
        public string OtherZoneHeight { get; set; } = string.Empty;


        //PALLET 
        public string PalletLength { get; set; } = string.Empty;
        public string PalletWidth { get; set; } = string.Empty;



        [Display(Name = "FECHA CREACION")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreatedAt { get; set; }

        public SystemConfiguration() { }

        public SystemConfiguration(CreateFirstConfigurationDTO FirstConfiguration) 
        {
            this.Sacks = FirstConfiguration.Sacks;
            this.IdClasses = FirstConfiguration.IdClasses;
            this.IdGrades = FirstConfiguration.IdGrades;
            this.SacksWeight = FirstConfiguration.SacksWeight;
            this.MinimunStocks = FirstConfiguration.MinimunStocks;
            this.MaximunStocks = FirstConfiguration.MaximunStocks;
            this.QuatitiesSacksPerLot = FirstConfiguration.QuatitiesSacksPerLot;
            this.WarehouseNames = FirstConfiguration.WarehouseNames;
            this.WarehouseDescription = FirstConfiguration.WarehouseDescription;
            this.WarehouseWidth = FirstConfiguration.WarehouseWidth;
            this.WarehouseLength = FirstConfiguration.WarehouseLength;
            this.WarehouseHeight = FirstConfiguration.WarehouseHeight;
            this.WarehouseRows = FirstConfiguration.WarehouseRows;
            this.WarehouseColumns = FirstConfiguration.WarehouseColumns;
            this.OtherZoneNames = FirstConfiguration.OtherZoneNames;
            this.OtherZoneDescription = FirstConfiguration.OtherZoneDescription;
            this.OtherZoneHeight = FirstConfiguration.OtherZoneHeight;
            this.OtherZoneLength = FirstConfiguration.OtherZoneLength;
            this.OtherZoneWidth = FirstConfiguration.OtherZoneWidth;
            this.PalletLength = FirstConfiguration.PalletLength;
            this.PalletWidth = FirstConfiguration.PalletWidth;
            this.CreatedAt = DateTime.Now;
        }

    }
}
