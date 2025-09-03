using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.UnitOfWork.Implementations
{
    public class AppearanceReferencesUnitOfWork : GenericUnitOfWork<AppearanceReference>, IAppearanceReferencesUnitOfWork
    {
        private readonly IAppearanceReferencesRepository _appearanceReferencesRepository;

        public AppearanceReferencesUnitOfWork(IGenericRepository<AppearanceReference> repository, IAppearanceReferencesRepository appearanceReferencesRepository) : base(repository)
        {
            _appearanceReferencesRepository = appearanceReferencesRepository;
        }
        public async Task<ActionResponse<IEnumerable<AppearanceReference>>> GetAsync(PaginationDTO pagination) => await _appearanceReferencesRepository.GetAsync(pagination);

        public async Task<IEnumerable<AppearanceReference>> GetComboAsync() => await _appearanceReferencesRepository.GetComboAsync();

        public async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination) => await _appearanceReferencesRepository.GetTotalPagesAsync(pagination);
    }
}
