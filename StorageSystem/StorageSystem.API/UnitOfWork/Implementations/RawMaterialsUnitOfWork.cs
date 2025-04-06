using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.Shared.Entities;

namespace StorageSystem.API.UnitOfWork.Implementations
{
    public class RawMaterialsUnitOfWork : GenericUnitOfWork<RawMaterial>, IRawMaterialsUnitOfWork
    {
        private readonly IRawMaterialsRepository _rawMaterialsRepository;

        public RawMaterialsUnitOfWork(IGenericRepository<RawMaterial> repository, IRawMaterialsRepository rawMaterialsRepository) : base(repository)
        {
            _rawMaterialsRepository = rawMaterialsRepository;
        }
        public async Task<IEnumerable<RawMaterial>> GetComboAsync() => await _rawMaterialsRepository.GetComboAsync();
    }
}
