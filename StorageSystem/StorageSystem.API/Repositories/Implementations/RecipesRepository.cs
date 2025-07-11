using Microsoft.EntityFrameworkCore;
using StorageSystem.API.Data;
using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.Repositories.Implementations
{
    public class RecipesRepository : GenericRepository<Recipe>, IRecipesRepository
    {
        private readonly DataContext _context;

        public RecipesRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public Task<ActionResponse<IEnumerable<Recipe>>> GetAsync(PaginationDTO pagination)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Recipe>> GetComboAsync()
        {
            return await _context.Recipes
                .OrderBy(r => r.Id)
                .ToListAsync();
        }

        public Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination)
        {
            throw new NotImplementedException();
        }
    }
}
