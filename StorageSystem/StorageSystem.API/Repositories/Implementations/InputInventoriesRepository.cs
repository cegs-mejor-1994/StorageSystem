using Microsoft.EntityFrameworkCore;
using StorageSystem.API.Data;
using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.Shared.Entities;

namespace StorageSystem.API.Repositories.Implementations
{
    public class InputInventoriesRepository : GenericRepository<InputInventory>, IInputInventoriesRepository
    {
        private readonly DataContext _context;

        public InputInventoriesRepository(DataContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<InputInventory>> GetWithRawMaterialsAndSuppliersAsync()
        {
            return await _context.InputInventories
                .OrderBy(i => i.Id)
                .Include(i => i.RawMaterial)
                .Include(i => i.Supplier)
                .Include(i => i.RawMaterial!.MeasurementUnit)
                .ToListAsync();
        }
    }
}
