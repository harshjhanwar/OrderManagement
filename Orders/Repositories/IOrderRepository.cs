using OrderManagement.Models;

namespace OrderManagement.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Orders>> GetAllAsync();
        Task<Orders> GetByIdAsync(int id);
        Task AddAsync(Orders order);
        Task UpdateAsync(Orders order);
        Task<bool> DeleteAsync(int id);
    }
}
