using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OptiArroz_Tesis_Proyect.Models.Entities
{
    public class Notification
    {
        [Key]
        public int IdNotification { get; set; }

        [Required]
        public string Message { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;

        [DefaultValue(1)]
        public int WasRead { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreatedAt { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ReadAt { get; set; }
        public string IdSendTo { get; set; } = string.Empty;
        public ApplicationUser? SentTo { get; set; }

        public int IdNotificationType { get; set; }
        public NotificationType? NotificationType { get; set; }
    }
}
