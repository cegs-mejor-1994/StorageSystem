using Microsoft.EntityFrameworkCore;
using StorageSystem.API.Data;
using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.Repositories.Implementations
{
    public class OrdersRepository : GenericRepository<Order>, IOrdersRepository
    {
        private readonly DataContext _context;

        public OrdersRepository(DataContext context) :base(context)
        {
            _context = context;
        }

        public Task<ActionResponse<IEnumerable<Order>>> GetAsync(PaginationDTO pagination)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Order>> GetComboAsync()
        {
            return await _context.Orders
                .OrderBy(o => o.Id)
                .ToListAsync();
        }

        public Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination)
        {
            throw new NotImplementedException();
        }
    }
}
