using Microsoft.EntityFrameworkCore;
using HeatExchangeApp.Models;

namespace HeatExchangeApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<CalculationParameters> CalculationParameters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CalculationParameters>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.LayerHeight).IsRequired();
                entity.Property(e => e.HeatTransferCoefficient).IsRequired();
                entity.Property(e => e.GasVelocity).IsRequired();
                entity.Property(e => e.GasHeatCapacity).IsRequired();
                entity.Property(e => e.MaterialConsumption).IsRequired();
                entity.Property(e => e.MaterialHeatCapacity).IsRequired();
                entity.Property(e => e.InitialMaterialTemp).IsRequired();
                entity.Property(e => e.InitialGasTemp).IsRequired();
                entity.Property(e => e.ApparatusDiameter).IsRequired();
                entity.Property(e => e.CreatedDate).IsRequired();
            });
        }
    }
}