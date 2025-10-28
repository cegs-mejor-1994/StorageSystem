using Microsoft.AspNetCore.Mvc;
using StorageSystem.API.UnitOfWork.Implementations;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.Shared.DTOs;
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

        [HttpGet]
        public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var response = await _clientsUnitOfWork.GetAsync(pagination);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

        [HttpGet("totalPages")]
        public override async Task<IActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            var action = await _clientsUnitOfWork.GetTotalPagesAsync(pagination);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }

        [HttpGet("combo")]
        public async Task<IActionResult> GetComboAsync()
        {
            return Ok(await _clientsUnitOfWork.GetComboAsync());
        }

        [HttpGet("getClientById")]
        public async Task<IActionResult> GetClientByID(string CNit)
        {
            var action = await _clientsUnitOfWork.GetClientById(CNit);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }
    }
}
