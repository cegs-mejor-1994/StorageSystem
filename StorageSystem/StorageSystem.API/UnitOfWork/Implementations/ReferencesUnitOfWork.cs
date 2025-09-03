using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.UnitOfWork.Implementations
{
    public class ReferencesUnitOfWork : GenericUnitOfWork<Reference>, IReferencesUnitOfWork
    {
        private readonly IReferencesRepository _rerefencesRepository;

        public ReferencesUnitOfWork(IGenericRepository<Reference> repository, IReferencesRepository rerefencesRepository) : base(repository)
        {
            _rerefencesRepository = rerefencesRepository;
        }

        public async Task<IEnumerable<Reference>> GetComboAsync() => await _rerefencesRepository.GetComboAsync();

        public async Task<ActionResponse<IEnumerable<Reference>>> GetAsync(PaginationDTO pagination) => await _rerefencesRepository.GetAsync(pagination);

        public async Task<IEnumerable<Reference>> GetWithTypeReferencesAndMeasurementUnitAsync() => await _rerefencesRepository.GetWithTypeReferencesAndMeasurementUnitAsync();

        public async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination) => await _rerefencesRepository.GetTotalPagesAsync(pagination);


    }
}
