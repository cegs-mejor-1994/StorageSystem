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
        public DbSet<Client> Clients { get; set; }
        public DbSet<InputInventory> InputInventories { get; set; }
        public DbSet<Manufactury> Manufacturies { get; set; }
        public DbSet<MeasurementUnit> MeasurementUnits { get; set; }                
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductsDetail> ProductsDetails { get; set; }
        public DbSet<ProductionGap> ProductionGaps { get; set; }
        public DbSet<RawMaterial> RawMaterials { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipeTotal> RecipeTotals { get; set; }
        public DbSet<Reference> References { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }                 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasIndex(c => c.Code).IsUnique();
            modelBuilder.Entity<Client>().HasIndex(cl => cl.Nit).IsUnique();
            modelBuilder.Entity<InputInventory>();
            modelBuilder.Entity<Manufactury>();
            modelBuilder.Entity<MeasurementUnit>().HasIndex(m => m.Code).IsUnique();
            modelBuilder.Entity<Order>();
            modelBuilder.Entity<Product>().HasIndex(p => p.Code ).IsUnique();
            modelBuilder.Entity<ProductsDetail>();
            modelBuilder.Entity<ProductionGap>();
            modelBuilder.Entity<RawMaterial>().HasIndex(r => r.Code).IsUnique();
            modelBuilder.Entity<Recipe>().HasIndex(r => new { r.ProductId, r.RawMaterialId }).IsUnique();
            modelBuilder.Entity<RecipeTotal>();
            modelBuilder.Entity<Reference>().HasIndex(r => new { r.MeasurementUnitId, r.Name }).IsUnique();
            modelBuilder.Entity<Supplier>().HasIndex(s => s.Nit).IsUnique();                       
            DisableCascadingDelete(modelBuilder);                                                                           
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
