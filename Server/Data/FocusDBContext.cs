using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using HardwareManagement.Server.Models.FocusDB;

namespace HardwareManagement.Server.Data
{
    public partial class FocusDBContext : DbContext
    {
        public FocusDBContext()
        {
        }

        public FocusDBContext(DbContextOptions<FocusDBContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative>()
              .HasOne(i => i.SiliconVendor)
              .WithMany(i => i.MicroControllerDerivatives)
              .HasForeignKey(i => i.SiliconVendorID)
              .HasPrincipalKey(i => i.SiliconVendorID);

            builder.Entity<HardwareManagement.Server.Models.FocusDB.MicroController>()
              .HasOne(i => i.AvailabilityStatus)
              .WithMany(i => i.MicroControllers)
              .HasForeignKey(i => i.AvailabilityStatusID)
              .HasPrincipalKey(i => i.AvailabilityStatusID);

            builder.Entity<HardwareManagement.Server.Models.FocusDB.MicroController>()
              .HasOne(i => i.MicroControllerDerivative)
              .WithMany(i => i.MicroControllers)
              .HasForeignKey(i => i.MicroControllerDerivativesID)
              .HasPrincipalKey(i => i.MicroControllerDerivativesID);

            builder.Entity<HardwareManagement.Server.Models.FocusDB.MicroController>()
              .HasOne(i => i.MicroControllerSubDerivative)
              .WithMany(i => i.MicroControllers)
              .HasForeignKey(i => i.MicroControllerSubDerivativesID)
              .HasPrincipalKey(i => i.MicroControllerSubDerivativesID);

            builder.Entity<HardwareManagement.Server.Models.FocusDB.MicroController>()
              .HasOne(i => i.SiliconVendor)
              .WithMany(i => i.MicroControllers)
              .HasForeignKey(i => i.SiliconVendorID)
              .HasPrincipalKey(i => i.SiliconVendorID);

            builder.Entity<HardwareManagement.Server.Models.FocusDB.TresosAcg>()
              .HasOne(i => i.Architecture)
              .WithMany(i => i.TresosAcgs)
              .HasForeignKey(i => i.ArchitectureID)
              .HasPrincipalKey(i => i.ArchitectureID);

            builder.Entity<HardwareManagement.Server.Models.FocusDB.TresosAcg>()
              .HasOne(i => i.Autosarversion)
              .WithMany(i => i.TresosAcgs)
              .HasForeignKey(i => i.AUTOSARVersionID)
              .HasPrincipalKey(i => i.AUTOSARVersionID);

            builder.Entity<HardwareManagement.Server.Models.FocusDB.TresosAcg>()
              .HasOne(i => i.AvailabilityStatus)
              .WithMany(i => i.TresosAcgs)
              .HasForeignKey(i => i.AvailabilityStatusID)
              .HasPrincipalKey(i => i.AvailabilityStatusID);

            builder.Entity<HardwareManagement.Server.Models.FocusDB.TresosAcg>()
              .HasOne(i => i.CompilerVendor)
              .WithMany(i => i.TresosAcgs)
              .HasForeignKey(i => i.CompilerVendorID)
              .HasPrincipalKey(i => i.CompilerVendorID);

            builder.Entity<HardwareManagement.Server.Models.FocusDB.TresosAcg>()
              .HasOne(i => i.CompilerVersion)
              .WithMany(i => i.TresosAcgs)
              .HasForeignKey(i => i.CompilerVersionID)
              .HasPrincipalKey(i => i.CompilerVersionID);

            builder.Entity<HardwareManagement.Server.Models.FocusDB.TresosAcg>()
              .HasOne(i => i.MicroControllerSubDerivative)
              .WithMany(i => i.TresosAcgs)
              .HasForeignKey(i => i.MicroControllerSubDerivativesID)
              .HasPrincipalKey(i => i.MicroControllerSubDerivativesID);

            builder.Entity<HardwareManagement.Server.Models.FocusDB.TresosAcg>()
              .HasOne(i => i.SiliconVendor)
              .WithMany(i => i.TresosAcgs)
              .HasForeignKey(i => i.SiliconVendorID)
              .HasPrincipalKey(i => i.SiliconVendorID);

            builder.Entity<HardwareManagement.Server.Models.FocusDB.TresosAcg>()
             .HasOne(i => i.MicroControllers)
             .WithMany(i => i.TresosAcgs)
             .HasForeignKey(i => i.MicroControllersID)
             .HasPrincipalKey(i => i.MicroControllersID);

            builder.Entity<HardwareManagement.Server.Models.FocusDB.TresosAutoCore>()
              .HasOne(i => i.Architecture)
              .WithMany(i => i.TresosAutoCores)
              .HasForeignKey(i => i.ArchitectureID)
              .HasPrincipalKey(i => i.ArchitectureID);

            builder.Entity<HardwareManagement.Server.Models.FocusDB.TresosAutoCore>()
              .HasOne(i => i.AvailabilityStatus)
              .WithMany(i => i.TresosAutoCores)
              .HasForeignKey(i => i.AvailabilityStatusID)
              .HasPrincipalKey(i => i.AvailabilityStatusID);

            builder.Entity<HardwareManagement.Server.Models.FocusDB.TresosAutoCore>()
              .HasOne(i => i.CompilerVendor)
              .WithMany(i => i.TresosAutoCores)
              .HasForeignKey(i => i.CompilerVendorID)
              .HasPrincipalKey(i => i.CompilerVendorID);

            builder.Entity<HardwareManagement.Server.Models.FocusDB.TresosAutoCore>()
              .HasOne(i => i.CompilerVersion)
              .WithMany(i => i.TresosAutoCores)
              .HasForeignKey(i => i.CompilerVersionID)
              .HasPrincipalKey(i => i.CompilerVersionID);

            builder.Entity<HardwareManagement.Server.Models.FocusDB.TresosAutoCore>()
              .HasOne(i => i.MicroControllerSubDerivative)
              .WithMany(i => i.TresosAutoCores)
              .HasForeignKey(i => i.MicroControllerSubDerivativesID)
              .HasPrincipalKey(i => i.MicroControllerSubDerivativesID);

            builder.Entity<HardwareManagement.Server.Models.FocusDB.TresosSafetyO>()
              .HasOne(i => i.Architecture)
              .WithMany(i => i.TresosSafetyOs)
              .HasForeignKey(i => i.ArchitectureID)
              .HasPrincipalKey(i => i.ArchitectureID);

            builder.Entity<HardwareManagement.Server.Models.FocusDB.TresosSafetyO>()
              .HasOne(i => i.AvailabilityStatus)
              .WithMany(i => i.TresosSafetyOs)
              .HasForeignKey(i => i.AvailabilityStatusID)
              .HasPrincipalKey(i => i.AvailabilityStatusID);

            builder.Entity<HardwareManagement.Server.Models.FocusDB.TresosSafetyO>()
              .HasOne(i => i.CompilerVendor)
              .WithMany(i => i.TresosSafetyOs)
              .HasForeignKey(i => i.CompilerVendorID)
              .HasPrincipalKey(i => i.CompilerVendorID);

            builder.Entity<HardwareManagement.Server.Models.FocusDB.TresosSafetyO>()
              .HasOne(i => i.CompilerVersion)
              .WithMany(i => i.TresosSafetyOs)
              .HasForeignKey(i => i.CompilerVersionID)
              .HasPrincipalKey(i => i.CompilerVersionID);

            builder.Entity<HardwareManagement.Server.Models.FocusDB.TresosSafetyO>()
              .HasOne(i => i.MicroControllerSubDerivative)
              .WithMany(i => i.TresosSafetyOs)
              .HasForeignKey(i => i.MicroControllerSubDerivativesID)
              .HasPrincipalKey(i => i.MicroControllerSubDerivativesID);
            this.OnModelBuilding(builder);
        }

        public DbSet<HardwareManagement.Server.Models.FocusDB.Architecture> Architectures { get; set; }

        public DbSet<HardwareManagement.Server.Models.FocusDB.AutosarVersion> AutosarVersions { get; set; }

        public DbSet<HardwareManagement.Server.Models.FocusDB.AvailabilityStatus> AvailabilityStatuses { get; set; }

        public DbSet<HardwareManagement.Server.Models.FocusDB.CompilerVendor> CompilerVendors { get; set; }

        public DbSet<HardwareManagement.Server.Models.FocusDB.CompilerVersion> CompilerVersions { get; set; }

        public DbSet<HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative> MicroControllerDerivatives { get; set; }

        public DbSet<HardwareManagement.Server.Models.FocusDB.MicroController> MicroControllers { get; set; }

        public DbSet<HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative> MicroControllerSubDerivatives { get; set; }

        public DbSet<HardwareManagement.Server.Models.FocusDB.SiliconVendor> SiliconVendors { get; set; }

        public DbSet<HardwareManagement.Server.Models.FocusDB.TresosAcg> TresosAcgs { get; set; }

        public DbSet<HardwareManagement.Server.Models.FocusDB.TresosAutoCore> TresosAutoCores { get; set; }

        public DbSet<HardwareManagement.Server.Models.FocusDB.TresosSafetyO> TresosSafetyOs { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
    
    }
}