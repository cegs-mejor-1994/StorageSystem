using Microsoft.EntityFrameworkCore;
using StorageSystem.API.Data;
using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.Repositories.Implementations
{
    public class ManufacturiesRepository : GenericRepository<Manufactury>, IManufacturiesRepository
    {
        private readonly DataContext _context;

        public ManufacturiesRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public Task<ActionResponse<IEnumerable<Manufactury>>> GetAsync(PaginationDTO pagination)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Manufactury>> GetComboAsync()
        {
            return await _context.Manufacturies
                .OrderBy(m => m.Id)
                .ToListAsync();
        }

        public Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination)
        {
            throw new NotImplementedException();
        }
    }
}
