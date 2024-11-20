namespace OptiArroz_Tesis_Proyect.Models.Dtos
{
    public class NotificationDTO
    {
        public int Id { get; set; }
        public string SentTo { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string MessageType { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public int WasRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ReadAt { get; set; }
    }
}
