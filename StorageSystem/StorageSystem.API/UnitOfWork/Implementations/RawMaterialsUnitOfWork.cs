using StorageSystem.API.Repositories.Implementations;
using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

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


        public async Task<ActionResponse<IEnumerable<RawMaterial>>> GetAsync(PaginationDTO pagination) => await _rawMaterialsRepository.GetAsync(pagination);

        public async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination) => await _rawMaterialsRepository.GetTotalPagesAsync(pagination);
    }
}
