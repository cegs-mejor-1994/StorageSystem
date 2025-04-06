using Microsoft.AspNetCore.Mvc;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.Shared.Entities;

namespace StorageSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeasurementUnitsController : GenericController<MeasurementUnit>
    {
        private readonly IMeasurementUnitsUnitOfWork _measurementUnitsUnitOfWork;

        public MeasurementUnitsController(IGenericUnitOfWork<MeasurementUnit> unitOfWork, IMeasurementUnitsUnitOfWork measurementUnitsUnitOfWork) : base(unitOfWork)
        {
            _measurementUnitsUnitOfWork = measurementUnitsUnitOfWork;
        }

        [HttpGet("combo")]
        public async Task<IActionResult> GetComboAsync()
        {
            return Ok(await _measurementUnitsUnitOfWork.GetComboAsync());
        }
    }
}
