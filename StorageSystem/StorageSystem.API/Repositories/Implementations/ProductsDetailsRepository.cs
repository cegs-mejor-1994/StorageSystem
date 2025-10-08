using Microsoft.EntityFrameworkCore;
using StorageSystem.API.Data;
using StorageSystem.API.Helpers;
using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.Repositories.Implementations
{
    public class ProductsDetailsRepository : GenericRepository<ProductsDetail>, IProductsDetailsRepository
    {
        private readonly DataContext _context;

        public ProductsDetailsRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ActionResponse<IEnumerable<ProductsDetail>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _context.ProductsDetails.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Product!.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            return new ActionResponse<IEnumerable<ProductsDetail>>
            {
                WasSuccess = true,
                Result = await queryable
                 .OrderBy(r => r.Id)
                 .Include(r => r.Reference)
                 .ThenInclude(mu => mu!.MeasurementUnit)
                 .Include(p => p.Product)
                 .ToListAsync()
            };
        }

        public async Task<IEnumerable<ProductsDetail>> GetComboAsync()
        {
            return await _context.ProductsDetails
                .OrderBy(r => r.Id)
                .Include(r => r!.Reference)
                .ThenInclude(mu => mu!.MeasurementUnit)
                .Include(p => p.Product)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductDetailDTO>> GetWithReferencesAsync()
        {
            return await _context.ProductsDetails
                .OrderBy(prD => prD.Product!.Name)
                .Select(prD => new ProductDetailDTO
                {
                    ProductDetailID = prD.Id,
                    productID = prD.Product!.Id,
                    FullName = prD.Product!.Name + " " + prD.Reference!.Name + " " + prD.Reference!.MeasurementUnit!.Code,
                    ReferenceValue = prD.Reference!.Name,
                    MeasurementUnitID = prD.Reference!.MeasurementUnitId,
                    PhysicalState = prD.Product.PhysicalState,
                    State = prD.State
                })
                .ToListAsync();
        }

        public async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination)
        {
            var queryable = _context.ProductsDetails.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Product!.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }
            double count = await queryable.CountAsync();
            int totalPages = (int)Math.Ceiling(count / pagination.RecordsNumber);
            return new ActionResponse<int>
            {
                WasSuccess = true,
                Result = totalPages
            };
        }

        public async Task<ActionResponse<ManufacturyDTO>> GetProductDetailGapRecipe(int ProductID)
        {
            var ManufacturyDTO = new ManufacturyDTO();
            var recipe = await _context.Recipes.FirstOrDefaultAsync(r => r.ProductId == ProductID);

            if (recipe == null)
            {
                return new ActionResponse<ManufacturyDTO>
                {
                    WasSuccess = false,
                    Message = "El producto no tiene receta asignada",
                    Result = ManufacturyDTO
                };
            }

            var productionGap = await _context.ProductionGaps.Where(pg => pg.RecipeId == recipe.Id).FirstOrDefaultAsync();
            if (productionGap == null)
            {
                return new ActionResponse<ManufacturyDTO>
                {
                    WasSuccess = false,
                    Message = "No hay bache asignado al producto",
                    Result = ManufacturyDTO
                };
            }

            ManufacturyDTO.ProductionGapID = productionGap.Id;
            ManufacturyDTO.RecipeID = recipe.Id;
            ManufacturyDTO.RecipeLeftAmount = productionGap.LeftAmount;
            

            return new ActionResponse<ManufacturyDTO>
            {
                WasSuccess = true,
                Result = ManufacturyDTO
            };
        }
    }
}
