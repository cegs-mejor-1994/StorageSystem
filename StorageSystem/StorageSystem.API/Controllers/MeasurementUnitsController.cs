using Microsoft.AspNetCore.Mvc;
using Shared.Entities;
using StorageSystem.API.UnitOfWork.Interfaces;

namespace StorageSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeasurementUnitsController : GenericController<MeasurementUnit>
    {
        private readonly IGenericUnitOfWork<MeasurementUnit> _unitOfWork;

        public MeasurementUnitsController(IGenericUnitOfWork<MeasurementUnit> unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
