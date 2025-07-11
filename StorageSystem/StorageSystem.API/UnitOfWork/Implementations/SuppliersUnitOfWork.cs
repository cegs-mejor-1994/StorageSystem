using StorageSystem.API.Repositories.Implementations;
using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.UnitOfWork.Implementations
{
    public class SuppliersUnitOfWork : GenericUnitOfWork<Supplier>, ISuppliersUnitOfWork
    {
        private readonly ISuppliersRepository _suppliersRepository;

        public SuppliersUnitOfWork(IGenericRepository<Supplier> repository, ISuppliersRepository suppliersRepository): base(repository)
        {
            _suppliersRepository = suppliersRepository;
        }

        public async Task<ActionResponse<IEnumerable<Supplier>>> GetAsync(PaginationDTO pagination) => await _suppliersRepository.GetAsync(pagination);

        public async Task<IEnumerable<Supplier>> GetComboAsync() => await _suppliersRepository.GetComboAsync();

        public async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination) => await _suppliersRepository.GetTotalPagesAsync(pagination);
    }
}
