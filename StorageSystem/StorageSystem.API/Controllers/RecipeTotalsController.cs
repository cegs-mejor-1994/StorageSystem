using Microsoft.AspNetCore.Mvc;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.Shared.Entities;

namespace StorageSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipeTotalsController : GenericController<RecipeTotal>
    {
        public RecipeTotalsController(IGenericUnitOfWork<RecipeTotal> unitOfWork) : base(unitOfWork)
        {
            
        }
    }
}
