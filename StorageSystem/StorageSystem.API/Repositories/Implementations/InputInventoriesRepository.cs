using Microsoft.EntityFrameworkCore;
using StorageSystem.API.Data;
using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.Repositories.Implementations
{
    public class InputInventoriesRepository : GenericRepository<InputInventory>, IInputInventoriesRepository
    {
        private readonly DataContext _context;

        public InputInventoriesRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public Task<ActionResponse<IEnumerable<InputInventory>>> GetAsync(PaginationDTO pagination)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<InputInventory>> GetWithRawMaterialsAndSuppliersAsync()
        {
            return await _context.InputInventories
                .OrderBy(i => i.RegisterDate)
                .Include(i => i.RawMaterial)
                .Include(i => i.Supplier)
                .Include(i => i.RawMaterial!.MeasurementUnit)
                .ToListAsync();
        }
    }
}
