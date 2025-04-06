using Microsoft.EntityFrameworkCore;
using StorageSystem.API.Data;
using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.Shared.Entities;

namespace StorageSystem.API.Repositories.Implementations
{
    public class SuppliersRepository : GenericRepository<Supplier>, ISuppliersRepository
    {
        private readonly DataContext _context;

        public SuppliersRepository(DataContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Supplier>> GetComboAsync()
        {
            return await _context.Suppliers
                .OrderBy(s => s.Name)
                .ToListAsync(); 
        }
    }
}
