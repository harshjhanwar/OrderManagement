using OrderManagement.Models;

namespace OrderManagement.OrderManagement.Business
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Orders>> GetAllAsync();
        Task<Orders?> GetByIdAsync(int id);
        Task<Result> AddAsync(Orders order);
        //Task UpdateAsync(Orders order);
        Task<Result> DeleteAsync(int id);
    }
}
