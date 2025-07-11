using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.Shared.Entities;

namespace StorageSystem.API.UnitOfWork.Implementations
{
    public class ManufacturiesUnitOfWork : GenericUnitOfWork<Manufactury>, IManufacturiesUnitOfWork
    {
        private readonly IManufacturiesRepository _manufacturiesRepository;

        public ManufacturiesUnitOfWork(IGenericRepository<Manufactury> repository, IManufacturiesRepository manufacturiesRepository) : base(repository)
        {
            _manufacturiesRepository = manufacturiesRepository;
        }

        public async Task<IEnumerable<Manufactury>> GetComboAsync() => await _manufacturiesRepository.GetComboAsync();
    }
}
