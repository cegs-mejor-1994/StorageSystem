using Microsoft.EntityFrameworkCore;
using StorageSystem.API.Data;
using StorageSystem.API.Helpers;
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

        public async Task<ActionResponse<IEnumerable<InputInventoryDTO>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _context.InputInventories.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => EF.Functions.Like(x.Product!.Name, $"%{pagination.Filter}%"));
                /*queryable = queryable.Where(x => x.Product!.Name.ToLower().Contains(pagination.Filter.ToLower()));*/
            }

            return new ActionResponse<IEnumerable<InputInventoryDTO>>
            {
                WasSuccess = true,
                Result = await queryable
                    .OrderBy(i => i.Id)
                    .Select(i => new InputInventoryDTO
                    {
                        ID = i.Id,
                        ControlCode = i.ControlCode,
                        Amount = i.Amount,
                        ProductName = i.Product!.Name,
                        MeasurementUnitCode = i.Product!.MeasurementUnit!.Code,
                        Batch = i.Batch,
                        MatutingDate = i.MatutingDate,
                        SupplierName = i.Product.RawMaterial != null && i.Product.RawMaterial.Supplier != null ? i.Product.RawMaterial.Supplier.Name! : string.Empty
                    })
                    .Paginate(pagination)
                    .ToListAsync()
            };
        }

        public async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination)
        {
            var queryable = _context.InputInventories.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => EF.Functions.Like(x.Product!.Name, $"%{pagination.Filter}%"));
                /*queryable = queryable.Where(x => x.Product!.Name.ToLower().Contains(pagination.Filter.ToLower()));*/
            }
            double count = await queryable.CountAsync();
            int totalPages = (int)Math.Ceiling(count / pagination.RecordsNumber);
            return new ActionResponse<int>
            {
                WasSuccess = true,
                Result = totalPages
            };
        }

        public async Task<IEnumerable<InputInventoryDTO>> GetWithRawMaterialsAndSuppliersAsync()
        {
            return await _context.InputInventories
                .OrderBy(i => i.Id)
                    .Select(i => new InputInventoryDTO
                    {
                        ID = i.Id,
                        ControlCode = i.ControlCode,
                        Amount = i.Amount,
                        ProductName = i.Product!.Name,
                        MeasurementUnitCode = i.Product!.MeasurementUnit!.Code,
                        Batch = i.Batch,
                        MatutingDate = i.MatutingDate,
                        SupplierName = i.Product.RawMaterial != null && i.Product.RawMaterial.Supplier != null ? i.Product.RawMaterial.Supplier.Name! : string.Empty
                    })
                .ToListAsync();
        }
    }
}
