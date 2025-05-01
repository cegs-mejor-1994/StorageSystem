using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.Shared.Entities;

namespace StorageSystem.API.UnitOfWork.Implementations
{
    public class ClientsUnitOfWork : GenericUnitOfWork<Client>, IClientsRepository
    {
        private readonly IClientsRepository _clientsRepository;

        public ClientsUnitOfWork(IGenericRepository<Client> repository, IClientsRepository clientsRepository) : base(repository)
        {
            _clientsRepository = clientsRepository;
        }

        public async Task<IEnumerable<Client>> GetComboAsync() => await _clientsRepository.GetComboAsync();
    }
}
