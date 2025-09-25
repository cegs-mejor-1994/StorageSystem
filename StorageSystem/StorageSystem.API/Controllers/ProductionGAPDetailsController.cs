using Microsoft.AspNetCore.Mvc;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.Shared.Entities;

namespace StorageSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductionGAPDetailsController : GenericController<ProductionGapDetail>
    {
        public ProductionGAPDetailsController(IGenericUnitOfWork<ProductionGapDetail> unitOfWork) : base(unitOfWork)    
        {
            
        }
    }
}
