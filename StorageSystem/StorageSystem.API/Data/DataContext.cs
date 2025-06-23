using Microsoft.EntityFrameworkCore;
using StorageSystem.Shared.Entities;

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
        public DbSet<Client> Clients { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Reference> References { get; set; }
        public DbSet<TypeReferenceProduct> TypeReferenceProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasIndex(c => c.Code).IsUnique();
            modelBuilder.Entity<RawMaterial>().HasIndex(r => r.Code).IsUnique();
            modelBuilder.Entity<Supplier>().HasIndex(s => s.Nit).IsUnique();
            modelBuilder.Entity<MeasurementUnit>().HasIndex(m => m.Code).IsUnique(); 
            modelBuilder.Entity<InputInventory>();
            DisableCascadingDelete(modelBuilder);
            modelBuilder.Entity<Client>().HasIndex(c => c.Nit).IsUnique();
            modelBuilder.Entity<Product>().HasIndex(p => p.Name).IsUnique();
            modelBuilder.Entity<Reference>().HasIndex(p => p.Name).IsUnique();
            modelBuilder.Entity<TypeReferenceProduct>().HasIndex(p => p.Name).IsUnique();
        }

        private void DisableCascadingDelete(ModelBuilder modelBuilder)
        {
            var relationships = modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys());
            foreach (var relationship in relationships)
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

    }
}
