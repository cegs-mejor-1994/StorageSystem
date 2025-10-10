using Microsoft.EntityFrameworkCore;
using StorageSystem.API.Data;
using StorageSystem.API.Helpers;
using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.Repositories.Implementations
{
    public class ProductionGapsRepository : GenericRepository<ProductionGap>, IProductionGapsRepository
    {
        private readonly DataContext _context;

        public ProductionGapsRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ActionResponse<IEnumerable<ProductionGap>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _context.ProductionGaps.AsQueryable();

            if (!string.IsNullOrEmpty(pagination.Filter))
            {
                queryable = queryable.Where(p => p.Recipe!.Product!.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            return new ActionResponse<IEnumerable<ProductionGap>>
            {
                WasSuccess = true,
                Result = await queryable
                    .OrderBy(p => p.RegisterDate)
                    .Include(p => p.Recipe)
                    .ThenInclude(r => r!.Product)
                    .Paginate(pagination)
                    .ToListAsync()
            };
        }

        public async Task<IEnumerable<ProductionGap>> GetComboAsync()
        {
            return await _context.ProductionGaps
                .OrderBy(p => p.RegisterDate)
                .Include(p => p.Recipe)
                .ThenInclude(r => r!.Product)
                .ToListAsync();
        }

        public async Task<ActionResponse<double>> GetProductionGapsByRecipeIdAsync(int recipeId)
        {
            var productionGap = await _context.ProductionGaps
                .Where(p => p.RecipeId == recipeId)                                
                .FirstOrDefaultAsync();

            if (productionGap != null) { 
                if(productionGap.LeftAmount > 0)
                {
                    return new ActionResponse<double>
                    {
                        WasSuccess = true,
                        Result = (double)productionGap.LeftAmount
                    };
                }
            }

            return new ActionResponse<double>
            {
                WasSuccess = true,
                Result = 0
            };
        }

        public async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination)
        {
            var queryable = _context.ProductionGaps.AsQueryable();

            if (!string.IsNullOrEmpty(pagination.Filter))
            {
                queryable = queryable.Where(p => p.Recipe!.Product!.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            double count = await queryable.CountAsync();
            int totalPages = (int)Math.Ceiling(count / pagination.RecordsNumber);
            return new ActionResponse<int>
            {
                WasSuccess = true,
                Result = totalPages
            };
        }
    }
}
