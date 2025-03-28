using Microsoft.EntityFrameworkCore;
using Shared.Entities;

namespace StorageSystem.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> context) : base(context)
        {
            
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<RawMaterial> RawMaterials { get; set; }
        public DbSet<MeasurementUnit> MeasurementUnits { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<InputInventory> InputInventories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasIndex(c => c.Code).IsUnique();
            modelBuilder.Entity<RawMaterial>().HasIndex(r => r.Code).IsUnique();
            modelBuilder.Entity<Supplier>().HasIndex(s => s.Nit).IsUnique();
            modelBuilder.Entity<MeasurementUnit>().HasIndex(m => m.Code).IsUnique(); 
            modelBuilder.Entity<InputInventory>();           
        }
    }
}
