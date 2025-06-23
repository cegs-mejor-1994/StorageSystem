using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.Shared.Entities;

namespace StorageSystem.API.UnitOfWork.Implementations
{
    public class TypeReferenceProductUnitOfWork : GenericUnitOfWork<TypeReferenceProduct>, ITypeReferenceProductUnitOfWork
    {
        private readonly ITypeReferenceProductRepository _typeReferenceProductRepository;

        public TypeReferenceProductUnitOfWork(IGenericRepository<TypeReferenceProduct> repository, ITypeReferenceProductRepository typeReferenceProductRepository) : base(repository)
        {
            _typeReferenceProductRepository = typeReferenceProductRepository;
        }

        public async Task<IEnumerable<TypeReferenceProduct>> GetComboAsync() => await _typeReferenceProductRepository.GetComboAsync();
    }
}
