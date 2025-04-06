using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.Shared.Entities;

namespace StorageSystem.API.UnitOfWork.Implementations
{
    public class MeasurementUnitsUnitOfWork : GenericUnitOfWork<MeasurementUnit>, IMeasurementUnitsUnitOfWork
    {
        private readonly IMeasurementUnitsRepository _measurementUnitsRepository;

        public MeasurementUnitsUnitOfWork(IGenericRepository<MeasurementUnit> repository, IMeasurementUnitsRepository measurementUnitsRepository): base(repository)
        {
            _measurementUnitsRepository = measurementUnitsRepository;
        }

        public async Task<IEnumerable<MeasurementUnit>> GetComboAsync() => await _measurementUnitsRepository.GetComboAsync();
    }
}
