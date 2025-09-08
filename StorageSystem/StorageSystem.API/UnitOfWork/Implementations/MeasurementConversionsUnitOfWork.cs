using StorageSystem.API.Repositories.Interfaces;
using StorageSystem.API.UnitOfWork.Interfaces;
using StorageSystem.Shared.Entities;

namespace StorageSystem.API.UnitOfWork.Implementations
{
    public class MeasurementConversionsUnitOfWork : GenericUnitOfWork<MeasurementConversion>, IMeasurementConversionsUnitOfWork
    {
        private readonly IMeasurementConversionsRepository _measurementConversionsRepository;

        public MeasurementConversionsUnitOfWork(IGenericRepository<MeasurementConversion> repository, IMeasurementConversionsRepository measurementConversionsRepository) : base(repository)
        {
            _measurementConversionsRepository = measurementConversionsRepository;
        }

        public async Task<IEnumerable<MeasurementConversion>> GetComboAsync() => await _measurementConversionsRepository.GetComboAsync();
    }
}
