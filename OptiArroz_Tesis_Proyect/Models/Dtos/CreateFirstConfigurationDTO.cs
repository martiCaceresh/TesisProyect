namespace OptiArroz_Tesis_Proyect.Models.Dtos
{
    public class CreateFirstConfigurationDTO
    {


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
    }
}
