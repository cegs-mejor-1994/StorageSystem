using Microsoft.AspNetCore.Mvc;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.Shared.Entities;

namespace StorageSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InputInventoriesController : GenericController<InputInventory>
    {        
        public InputInventoriesController(IGenericUnitOfWork<InputInventory> unitOfWork): base(unitOfWork)
        {
            
        }
    }
}
