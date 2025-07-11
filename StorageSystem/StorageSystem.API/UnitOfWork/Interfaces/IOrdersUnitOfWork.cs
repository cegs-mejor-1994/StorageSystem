using StorageSystem.Shared.Entities;

namespace StorageSystem.API.UnitOfWork.Interfaces
{
    public interface IOrdersUnitOfWork
    {
        Task<IEnumerable<Order>> GetComboAsync();
    }
}
