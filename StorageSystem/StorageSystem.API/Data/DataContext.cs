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
        public DbSet<MeasurementConversion> MeasurementConversions { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductsDetail> ProductsDetails { get; set; }
        public DbSet<ProductionGap> ProductionGaps { get; set; }
        public DbSet<RawMaterial> RawMaterials { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipeDetail> RecipeDetails { get; set; }        
        public DbSet<Reference> References { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }                 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasIndex(pr => pr.Code).IsUnique();
            modelBuilder.Entity<Category>().HasIndex(pr => pr.Name).IsUnique();
            modelBuilder.Entity<Client>().HasIndex(cl => cl.Nit).IsUnique();
            modelBuilder.Entity<InputInventory>();
            modelBuilder.Entity<Manufactury>();
            modelBuilder.Entity<MeasurementConversion>().HasIndex(mc => new { mc.FromUnitId, mc.ToUnitId }).IsUnique();
            modelBuilder.Entity<MeasurementConversion>()
                .HasOne(mc => mc.FromUnit)
                .WithMany(mu => mu.ConversionsFrom)
                .HasForeignKey(mc => mc.FromUnitId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MeasurementConversion>()
                .HasOne(mc => mc.ToUnit)
                .WithMany(mu => mu.ConversionsTo)
                .HasForeignKey(mc => mc.ToUnitId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<MeasurementUnit>().HasIndex(mu => mu.Code).IsUnique();
            modelBuilder.Entity<MeasurementUnit>().HasIndex(mu => mu.Name).IsUnique();
            modelBuilder.Entity<Order>();
            modelBuilder.Entity<Product>().HasIndex(pr => pr.Code).IsUnique();
            modelBuilder.Entity<Product>().HasIndex(pr => pr.Name).IsUnique();
            modelBuilder.Entity<ProductionGap>();
            modelBuilder.Entity<ProductsDetail>().HasIndex(prd => new { prd.ProductId, prd.ReferenceId }).IsUnique();   
            modelBuilder.Entity<RawMaterial>().HasIndex(ra => new { ra.ProductId, ra.SupplierId }).IsUnique();
            modelBuilder.Entity<Recipe>().HasIndex(rec =>  rec.ProductId).IsUnique();
            modelBuilder.Entity<RecipeDetail>().HasIndex(recd => new { recd.ProductId, recd.RecipeId });
            modelBuilder.Entity<Reference>().HasIndex(refe => new { refe.MeasurementUnitId, refe.Name }).IsUnique();
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
