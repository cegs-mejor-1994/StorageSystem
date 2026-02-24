using StorageSystem.API.Repositories.Implementations;
using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.UnitOfWork.Implementations
{
    public class InputInventoriesUnitOfWork : GenericUnitOfWork<InputInventory>, IInputInventoriesUnitOfWork
    {
        private readonly IInputInventoriesRepository _inputInventoriesRepository;

        public InputInventoriesUnitOfWork(IGenericRepository<InputInventory> repository, IInputInventoriesRepository inputInventoriesRepository) : base(repository)
        {
            _inputInventoriesRepository = inputInventoriesRepository;
        }

        public async Task<IEnumerable<InputInventory>> GetWithRawMaterialsAndSuppliersAsync() => await _inputInventoriesRepository.GetWithRawMaterialsAndSuppliersAsync();

        public async Task<ActionResponse<IEnumerable<InputInventoryDTO>>> GetAsync(PaginationDTO pagination) => await _inputInventoriesRepository.GetAsync(pagination);

        public async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination) => await _inputInventoriesRepository.GetTotalPagesAsync(pagination);
    }
}
