namespace OptiArroz_Tesis_Proyect.Models.Dtos
{
    public class ZoneDTO
    {
        public int IdZone { get; set; }

        public string Name { get; set; } = string.Empty;
        
        public int Rows { get; set; }

        public int Columns { get; set; }

        public List<UbicationDTO> Ubications { get; set; } = new List<UbicationDTO>();
    }
}
