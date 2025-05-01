using Microsoft.AspNetCore.Mvc;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.Shared.Entities;

namespace StorageSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : GenericController<Client>
    {
        private readonly IClientsUnitOfWork _clientsUnitOfWork;

        public ClientsController(IGenericUnitOfWork<Client> unitOfWork, IClientsUnitOfWork clientsUnitOfWork) : base(unitOfWork)
        {
            _clientsUnitOfWork = clientsUnitOfWork;
        }

        [HttpGet("combo")]
        public async Task<IActionResult> GetComboAsync()
        {
            return Ok(await _clientsUnitOfWork.GetComboAsync());
        }
    }
}
