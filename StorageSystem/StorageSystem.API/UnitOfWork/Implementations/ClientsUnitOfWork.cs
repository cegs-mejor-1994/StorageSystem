using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.Shared.Responses;

namespace StorageSystem.API.UnitOfWork.Implementations
{
    public class ClientsUnitOfWork : GenericUnitOfWork<Client>, IClientsUnitOfWork
    {
        private readonly IClientsRepository _clientsRepository;

        public ClientsUnitOfWork(IGenericRepository<Client> repository, IClientsRepository clientsRepository) : base(repository)
        {
            _clientsRepository = clientsRepository;
        }

        public async Task<ActionResponse<IEnumerable<Client>>> GetAsync(PaginationDTO pagination) => await _clientsRepository.GetAsync(pagination);

        public async Task<ActionResponse<int>> GetClientById(string CNit) => await _clientsRepository.GetClientById(CNit);

        public async Task<IEnumerable<Client>> GetComboAsync() => await _clientsRepository.GetComboAsync();

        public async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination) => await _clientsRepository.GetTotalPagesAsync(pagination);
    }
}
