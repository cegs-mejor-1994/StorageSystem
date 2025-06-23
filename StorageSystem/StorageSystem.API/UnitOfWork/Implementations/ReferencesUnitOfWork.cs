using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.API.UnitOfWork.Interfaces;
using System.Security.Cryptography.Xml;

namespace StorageSystem.API.UnitOfWork.Implementations
{
    public class ReferencesUnitOfWork : GenericUnitOfWork<Reference>, IReferencesUnitOfWork
    {
        private readonly IRerefencesRepository _rerefencesRepository;

        public ReferencesUnitOfWork(IGenericRepository<Reference> repository, IRerefencesRepository rerefencesRepository) : base(repository)
        {
            _rerefencesRepository = rerefencesRepository;
        }

        public async Task<IEnumerable<Shared.Entities.Reference>> GetComboAsync() => await _rerefencesRepository.GetComboAsync();
    }
}
