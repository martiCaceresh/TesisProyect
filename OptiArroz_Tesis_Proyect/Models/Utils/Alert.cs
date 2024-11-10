namespace OptiArroz_Tesis_Proyect.Models.Utils
{
    public class AlertModel
    {
        public string Type { get; set; }

        public string Title { get; set; }
        public string Message { get; set; }

        public AlertModel(string type, string title, string message)
        {
            Type = type;
            Message = message;
            Title = title;
        }
    }
}

