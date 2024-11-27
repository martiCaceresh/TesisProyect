using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Models.ViewModels
{
    public class AlertManagerVM
    {
        public NotificationType Stock { get; set; } = new NotificationType();
        public NotificationType Expiration { get; set; } = new NotificationType();
    }
}
