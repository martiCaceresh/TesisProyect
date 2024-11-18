using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OptiArroz_Tesis_Proyect.Models.Entities;

namespace OptiArroz_Tesis_Proyect.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("Server=optiarroz.cp2g64qkmqw5.us-east-1.rds.amazonaws.com;Database=OptiArroz;User=admin;Password=Marinf2024;",
                    new MySqlServerVersion(new Version(8, 0, 30))); // Ajusta la versión a la que estés utilizando
            }
        }

        public ApplicationDbContext() { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region One to One

            // Configurar la relación uno a uno entre Ubicacion y Lote (LoteActual)
            builder.Entity<Ubication>()
                .HasOne(u => u.CurrentRiceLot)
                .WithOne(l => l.LastUbication)
                .HasForeignKey<Ubication>(u => u.IdCurrentRiceLot)  // Clave foránea en Ubicacion
                .OnDelete(DeleteBehavior.SetNull); // Permitir valores null si la ubicación está vacía

            // Relación inversa: Lote tiene un UltimaUbicacion
            builder.Entity<RiceLot>()
                .HasOne(l => l.LastUbication)
                .WithOne(u => u.CurrentRiceLot)
                .HasForeignKey<RiceLot>(l => l.IdLastUbication); // Clave foránea en Lote

            #endregion

            #region One to Many

            // RICE LOT
            builder.Entity<RiceLot>()
                .HasOne(p => p.RiceClassification)
                .WithMany(f => f.RiceLots)
                .HasForeignKey(p => p.IdClassification);

            builder.Entity<RiceLot>()
                .HasOne(p => p.Zone)
                .WithMany(f => f.RiceLots)
                .HasForeignKey(p => p.IdZone);

            builder.Entity<RiceLot>()
                .HasOne(p => p.CreatedBy)
                .WithMany(f => f.RiceLotsCreated)
                .HasForeignKey(p => p.IdCreatedBy);

            builder.Entity<RiceLot>()
                .HasOne(p => p.UpdatedBy)
                .WithMany(f => f.RiceLotsUpdated)
                .HasForeignKey(p => p.IdUpdatedBy);

            // RICE SACK

            builder.Entity<RiceSack>()
                .HasOne(p => p.CreatedBy)
                .WithMany(f => f.RiceSacksCreated)
                .HasForeignKey(p => p.IdCreatedBy);

            builder.Entity<RiceSack>()
                .HasOne(p => p.UpdatedBy)
                .WithMany(f => f.RiceSacksUpdated)
                .HasForeignKey(p => p.IdUpdatedBy);

            // RICE CLASS

            builder.Entity<RiceClass>()
                .HasOne(p => p.CreatedBy)
                .WithMany(f => f.RiceClassesCreated)
                .HasForeignKey(p => p.IdCreatedBy);

            builder.Entity<RiceClass>()
                .HasOne(p => p.UpdatedBy)
                .WithMany(f => f.RiceClassesUpdated)
                .HasForeignKey(p => p.IdUpdatedBy);

            // RICE GRADE

            builder.Entity<RiceGrade>()
                .HasOne(p => p.CreatedBy)
                .WithMany(f => f.RiceGradesCreated)
                .HasForeignKey(p => p.IdCreatedBy);

            builder.Entity<RiceGrade>()
                .HasOne(p => p.UpdatedBy)
                .WithMany(f => f.RiceGradesUpdated)
                .HasForeignKey(p => p.IdUpdatedBy);


            // RICE CLASSIFICATION

            builder.Entity<RiceClassification>()
                .HasOne(p => p.RiceGrade)
                .WithMany(f => f.RiceClassifications)
                .HasForeignKey(p => p.IdRiceGrade);

            builder.Entity<RiceClassification>()
                .HasOne(p => p.RiceClass)
                .WithMany(f => f.RiceClassifications)
                .HasForeignKey(p => p.IdRiceClass);

            builder.Entity<RiceClassification>()
                .HasOne(p => p.RiceSack)
                .WithMany(f => f.RiceClassifications)
                .HasForeignKey(p => p.IdRiceSack);

            builder.Entity<RiceClassification>()
                .HasOne(p => p.CreatedBy)
                .WithMany(f => f.RiceClassificationsCreated)
                .HasForeignKey(p => p.IdCreatedBy);

            builder.Entity<RiceClassification>()
                .HasOne(p => p.UpdatedBy)
                .WithMany(f => f.RiceClassificationsUpdated)
                .HasForeignKey(p => p.IdUpdatedBy);

            // RICE MOVEMENT

            builder.Entity<RiceLotMovement>()
                .HasOne(p => p.RiceLot)
                .WithMany(f => f.RiceLotMovements)
                .HasForeignKey(p => p.IdRiceLot);

            builder.Entity<RiceLotMovement>()
                .HasOne(p => p.Origin)
                .WithMany(f => f.RiceLotMovementOrigins)
                .HasForeignKey(p => p.IdOrigin);

            builder.Entity<RiceLotMovement>()
                .HasOne(p => p.Destination)
                .WithMany(f => f.RiceLotMovementDestinations)
                .HasForeignKey(p => p.IdDestination);


            builder.Entity<RiceLotMovement>()
                .HasOne(p => p.ZoneOrigin)
                .WithMany(f => f.RiceLotMovementZoneOrigins)
                .HasForeignKey(p => p.IdZoneOrigin);

            builder.Entity<RiceLotMovement>()
                .HasOne(p => p.ZoneDestination)
                .WithMany(f => f.RiceLotMovementZoneDestinations)
                .HasForeignKey(p => p.IdZoneDestination);

            builder.Entity<RiceLotMovement>()
                .HasOne(p => p.CreatedBy)
                .WithMany(f => f.RiceLotMovementsCreated)
                .HasForeignKey(p => p.IdCreatedBy);

            builder.Entity<RiceLotMovement>()
                .HasOne(p => p.UpdatedBy)
                .WithMany(f => f.RiceLotMovementsUpdated)
                .HasForeignKey(p => p.IdUpdatedBy);

            // UBICATION

            builder.Entity<Ubication>()
                .HasOne(p => p.Zone)
                .WithMany(f => f.Ubications)
                .HasForeignKey(p => p.IdZone);

            // ZONE

            builder.Entity<Zone>()
                .HasOne(p => p.CreatedBy)
                .WithMany(f => f.ZonesCreated)
                .HasForeignKey(p => p.IdCreatedBy);

            builder.Entity<Zone>()
                .HasOne(p => p.UpdatedBy)
                .WithMany(f => f.ZonesUpdated)
                .HasForeignKey(p => p.IdUpdatedBy);

            // RICE SACKS DEVOLUTION TYPE

            builder.Entity<RiceSacksDevolutionType>()
                .HasOne(p => p.CreatedBy)
                .WithMany(f => f.RiceSacksDevolutionTypesCreated)
                .HasForeignKey(p => p.IdCreatedBy);

            builder.Entity<RiceSacksDevolutionType>()
                .HasOne(p => p.UpdatedBy)
                .WithMany(f => f.RiceSacksDevolutionTypesUpdated)
                .HasForeignKey(p => p.IdUpdatedBy);

            // RICE SACKS DEVOLUTION

            builder.Entity<RiceSacksDevolution>()
                .HasOne(p => p.RiceSacksDevolutionType)
                .WithMany(f => f.RiceSacksDevolutions)
                .HasForeignKey(p => p.IdRiceSacksDevolutionType);

            builder.Entity<RiceSacksDevolution>()
                .HasOne(p => p.CreatedBy)
                .WithMany(f => f.RiceSacksDevolutionsCreated)
                .HasForeignKey(p => p.IdCreatedBy);

            builder.Entity<RiceSacksDevolution>()
                .HasOne(p => p.UpdatedBy)
                .WithMany(f => f.RiceSacksDevolutionsUpdated)
                .HasForeignKey(p => p.IdUpdatedBy);

            // RICE SACKS DEVOLUTION DETAIL

            builder.Entity<RiceSacksDevolutionDetail>()
                .HasOne(p => p.RiceSacksDevolution)
                .WithMany(f => f.RiceSacksDevolutionDetails)
                .HasForeignKey(p => p.IdRiceSacksDevolution);

            builder.Entity<RiceSacksDevolutionDetail>()
                .HasOne(p => p.RiceLot)
                .WithMany(f => f.RiceSacksDevolutionDetails)
                .HasForeignKey(p => p.IdRiceLot);

            // RICE SACKS OUTPUT TYPE

            builder.Entity<RiceSacksOutputType>()
                .HasOne(p => p.CreatedBy)
                .WithMany(f => f.RiceSacksOutputTypesCreated)
                .HasForeignKey(p => p.IdCreatedBy);

            builder.Entity<RiceSacksOutputType>()
                .HasOne(p => p.UpdatedBy)
                .WithMany(f => f.RiceSacksOutputTypesUpdated)
                .HasForeignKey(p => p.IdUpdatedBy);

            // RICE SACKS OUTPUT

            builder.Entity<RiceSacksOutput>()
                .HasOne(p => p.RiceSacksOutputType)
                .WithMany(f => f.RiceSacksOutputs)
                .HasForeignKey(p => p.IdRiceSacksOutputType);

            builder.Entity<RiceSacksOutput>()
                .HasOne(p => p.CreatedBy)
                .WithMany(f => f.RiceSacksOutputsCreated)
                .HasForeignKey(p => p.IdCreatedBy);

            builder.Entity<RiceSacksOutput>()
                .HasOne(p => p.UpdatedBy)
                .WithMany(f => f.RiceSacksOutputsUpdated)
                .HasForeignKey(p => p.IdUpdatedBy);

            // RICE SACKS OUTPUT DETAIL

            builder.Entity<RiceSacksOutputDetail>()
                .HasOne(p => p.RiceSacksOutput)
                .WithMany(f => f.RiceSacksOutputDetails)
                .HasForeignKey(p => p.IdRiceSacksOutput);

            builder.Entity<RiceSacksOutputDetail>()
                .HasOne(p => p.RiceLot)
                .WithMany(f => f.RiceSacksOutputDetails)
                .HasForeignKey(p => p.IdRiceLot);


            //NOTIFICATION
            builder.Entity<Notification>()
                .HasOne(p => p.NotificationType)
                .WithMany(f => f.Notifications)
                .HasForeignKey(p => p.IdNotificationType);

            builder.Entity<Notification>()
                .HasOne(p => p.SentTo)
                .WithMany(f => f.Notifications)
                .HasForeignKey(p => p.IdSendTo);


            #endregion

        }


        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<NotificationType> NotificationTypes { get; set; }
        public virtual DbSet<RiceClass> RiceClasses { get; set; }
        public virtual DbSet<RiceClassification> RiceClassifications { get; set; }
        public virtual DbSet<RiceGrade> RiceGrades { get; set; }
        public virtual DbSet<RiceLot> RiceLots { get; set; }
        public virtual DbSet<RiceLotMovement> RiceLotMovements { get; set; }
        public virtual DbSet<RiceSack> RiceSacks { get; set; }
        public virtual DbSet<RiceSacksDevolution> RiceSacksDevolutions { get; set; }
        public virtual DbSet<RiceSacksDevolutionDetail> RiceSacksDevolutionDetails { get; set; }
        public virtual DbSet<RiceSacksDevolutionType> RiceSacksDevolutionTypes { get; set; }
        public virtual DbSet<RiceSacksOutput> RiceSacksOutputs { get; set; }
        public virtual DbSet<RiceSacksOutputDetail> RiceSacksOutputDetails { get; set; }
        public virtual DbSet<RiceSacksOutputType> RiceSacksOutputTypes { get; set; }
        public virtual DbSet<Ubication> Ubications { get; set; }
        public virtual DbSet<Zone> Zones { get; set; }
        public virtual DbSet<SystemConfiguration> SystemConfiguration { get; set; }

    }
}
