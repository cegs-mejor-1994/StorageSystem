using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.UnitOfWork.Implementations
{
    public class ManufacturiesUnitOfWork : GenericUnitOfWork<Manufactury>, IManufacturiesUnitOfWork
    {
        private readonly IManufacturiesRepository _manufacturiesRepository;

        public ManufacturiesUnitOfWork(IGenericRepository<Manufactury> repository, IManufacturiesRepository manufacturiesRepository) : base(repository)
        {
            _manufacturiesRepository = manufacturiesRepository;
        }

        public async Task<ActionResponse<IEnumerable<Manufactury>>> GetAsync(PaginationDTO pagination) => await _manufacturiesRepository.GetAsync(pagination);

        public async Task<IEnumerable<Manufactury>> GetComboAsync() => await _manufacturiesRepository.GetComboAsync();

        public async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination) => await _manufacturiesRepository.GetTotalPagesAsync(pagination);
    }
}
