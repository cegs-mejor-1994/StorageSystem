using StorageSystem.Shared.Entities;

namespace StorageSystem.API.UnitOfWork.Interfaces
{
    public interface IMeasurementUnitsUnitOfWork
    {
        Task<IEnumerable<MeasurementUnit>> GetComboAsync();
    }
}
