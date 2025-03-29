using StorageSystem.Shared.Entities;

namespace StorageSystem.API.Repositories.Interfaces
{
    public interface IMeasurementUnitsRepository
    {
        Task <IEnumerable<MeasurementUnit>> GetComboAsync();
    }
}
