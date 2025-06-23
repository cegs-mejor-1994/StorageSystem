using Microsoft.EntityFrameworkCore;
using StorageSystem.API.Data;
using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.Shared.Entities;

namespace StorageSystem.API.Repositories.Implementations
{
    public class TypeReferenceProductRepository : GenericRepository<TypeReferenceProduct>, ITypeReferenceProductRepository
    {
        private readonly DataContext _context;

        public TypeReferenceProductRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TypeReferenceProduct>> GetComboAsync()
        {
            return await _context.TypeReferenceProducts
                .OrderBy(t => t.Name)
                .ToListAsync();
        }
    }
}
