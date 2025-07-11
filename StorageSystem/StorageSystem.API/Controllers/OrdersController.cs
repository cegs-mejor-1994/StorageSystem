using Microsoft.AspNetCore.Mvc;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.Shared.Entities;

namespace StorageSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : GenericController<Order>
    {
        private readonly IOrdersUnitOfWork _ordersUnitOfWork;

        public OrdersController(IGenericUnitOfWork<Order> unitOfWork, IOrdersUnitOfWork ordersUnitOfWork) : base(unitOfWork)
        {
            _ordersUnitOfWork = ordersUnitOfWork;
        }

        [HttpGet("combo")]
        public async Task<IActionResult> GetComboAsync()
        {
            return Ok(await _ordersUnitOfWork.GetComboAsync());
        }
    }
}
