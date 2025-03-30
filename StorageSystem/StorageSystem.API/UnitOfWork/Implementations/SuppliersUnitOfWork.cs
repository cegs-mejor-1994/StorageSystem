using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.Shared.Entities;

namespace StorageSystem.API.UnitOfWork.Implementations
{
    public class SuppliersUnitOfWork : GenericUnitOfWork<Supplier>, ISuppliersUnitOfWork
    {
        private readonly ISuppliersRepository _suppliersRepository;

        public SuppliersUnitOfWork(IGenericRepository<Supplier> repository, ISuppliersRepository suppliersRepository): base(repository)
        {
            _suppliersRepository = suppliersRepository;
        }
  
        public async Task<IEnumerable<Supplier>> GetComboAsync() => await _suppliersRepository.GetComboAsync();
    }
}
