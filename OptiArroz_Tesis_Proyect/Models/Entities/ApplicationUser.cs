using Microsoft.AspNetCore.Identity;

namespace OptiArroz_Tesis_Proyect.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public DateTime RegisterDate { get; set; }

        public string PasswordResetCode { get; set; }= string.Empty;
        public DateTime? CodeExpiration { get; set; }

        public List<Notification>? Notifications { get; set; }
        public List<RiceClass>? RiceClassesUpdated { get; set; }
        public List<RiceClass>? RiceClassesCreated { get; set; }

        public List<RiceClassification>? RiceClassificationsUpdated { get; set; }
        public List<RiceClassification>? RiceClassificationsCreated { get; set; }

        public List<RiceGrade>? RiceGradesUpdated { get; set; }
        public List<RiceGrade>? RiceGradesCreated { get; set; }

        public List<RiceLot>? RiceLotsUpdated { get; set; }
        public List<RiceLot>? RiceLotsCreated { get; set; }

        public List<RiceLotMovement>? RiceLotMovementsUpdated { get; set; }
        public List<RiceLotMovement>? RiceLotMovementsCreated { get; set; }

        public List<RiceSack>? RiceSacksUpdated { get; set; }
        public List<RiceSack>? RiceSacksCreated { get; set; }

        public List<RiceSacksDevolution>? RiceSacksDevolutionsUpdated { get; set; }
        public List<RiceSacksDevolution>? RiceSacksDevolutionsCreated { get; set; }

        public List<RiceSacksDevolutionType>? RiceSacksDevolutionTypesUpdated { get; set; }
        public List<RiceSacksDevolutionType>? RiceSacksDevolutionTypesCreated { get; set; }

        public List<RiceSacksOutput>? RiceSacksOutputsUpdated { get; set; }
        public List<RiceSacksOutput>? RiceSacksOutputsCreated { get; set; }

        public List<RiceSacksOutputType>? RiceSacksOutputTypesUpdated { get; set; }
        public List<RiceSacksOutputType>? RiceSacksOutputTypesCreated { get; set; }

        public List<Zone>? ZonesUpdated { get; set; }
        public List<Zone>? ZonesCreated { get; set; }

    }
}
