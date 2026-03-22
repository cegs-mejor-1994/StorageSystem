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
        public DbSet<ProductionGap> ProductionGaps { get; set; }
        public DbSet<ProductionGapDetail> ProductionGapDetails { get; set; }
        public DbSet<ProductsDetail> ProductsDetails { get; set; }
        public DbSet<ProductsDetailStructure> ProductsDetailStructures { get; set; }
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
            modelBuilder.Entity<Product>()
                .HasOne(p => p.RawMaterial)
                .WithOne(r => r.Product)
                .HasForeignKey<RawMaterial>(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ProductionGap>();
            modelBuilder.Entity<ProductsDetail>().HasIndex(prd => new { prd.ProductId, prd.ReferenceId }).IsUnique();
            modelBuilder.Entity<ProductionGapDetail>(entity =>
            {
                entity.HasOne(pgd => pgd.ProductionGap)
                    .WithMany(pg => pg.ProductionGapDetails)
                    .HasForeignKey(pgd => pgd.ProductionGapId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(pgd => pgd.InputInventory)
                    .WithMany(ii => ii.ProductionGapDetails)
                    .HasForeignKey(pgd => pgd.InputInventoryId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(pgd => pgd.ProductionGapSource)
                    .WithMany()
                    .HasForeignKey(pgd => pgd.ProductionGapSourceId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(e => e.InputInventoryId)
                    .HasFilter("[InputInventoryId] IS NOT NULL");

                entity.HasIndex(e => e.ProductionGapSourceId)
                    .HasFilter("[ProductionGapSourceId] IS NOT NULL");
                entity.ToTable(t =>
                {
                    t.HasCheckConstraint(
                        "CK_ProductionRelations_Source",
                        "(InputInventoryId IS NOT NULL AND ProductionGapSourceId IS NULL) OR (InputInventoryId IS NULL AND ProductionGapSourceId IS NOT NULL)"
                    );
                });
            });

            modelBuilder.Entity<ProductsDetailStructure>().HasIndex(prds => new { prds.ProductsDetailId, prds.ProductId }).IsUnique();
            modelBuilder.Entity<RawMaterial>().HasIndex(ra => new { ra.ProductId, ra.SupplierId }).IsUnique();
            modelBuilder.Entity<RawMaterial>()
                .HasOne(r => r.Supplier)
                .WithMany(s => s.RawMaterials)
                .HasForeignKey(r => r.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);
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
