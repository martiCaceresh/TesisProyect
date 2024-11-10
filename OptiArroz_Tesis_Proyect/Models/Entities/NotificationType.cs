using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace OptiArroz_Tesis_Proyect.Models.Entities
{
    public class NotificationType
    {
        [Key]
        public int IdNotificationType { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public int PriorNotificationDays { get; set; }

        public int FrecuencyAfterFirstNotification { get; set; }

        public List<Notification>? Notifications { get; set; }

    }
}
