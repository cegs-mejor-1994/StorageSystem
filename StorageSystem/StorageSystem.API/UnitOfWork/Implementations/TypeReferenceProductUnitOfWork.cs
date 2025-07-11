using StorageSystem.API.Repositories.Implementations;
using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.UnitOfWork.Implementations
{
    public class TypeReferenceProductUnitOfWork : GenericUnitOfWork<TypeReferenceProduct>, ITypeReferenceProductUnitOfWork
    {
        private readonly ITypeReferenceProductRepository _typeReferenceProductRepository;

        public TypeReferenceProductUnitOfWork(IGenericRepository<TypeReferenceProduct> repository, ITypeReferenceProductRepository typeReferenceProductRepository) : base(repository)
        {
            _typeReferenceProductRepository = typeReferenceProductRepository;
        }

        public async Task<ActionResponse<IEnumerable<TypeReferenceProduct>>> GetAsync(PaginationDTO pagination) => await _typeReferenceProductRepository.GetAsync(pagination);

        public async Task<IEnumerable<TypeReferenceProduct>> GetComboAsync() => await _typeReferenceProductRepository.GetComboAsync();

        public async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination) => await _typeReferenceProductRepository.GetTotalPagesAsync(pagination);
    }
}
