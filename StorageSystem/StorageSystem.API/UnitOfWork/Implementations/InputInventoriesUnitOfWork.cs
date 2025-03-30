using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.Shared.Entities;

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
    }
}
