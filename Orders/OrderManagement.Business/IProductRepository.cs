using OrderManagement.Models;

namespace OrderManagement.OrderManagement.Business
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProducts(int orderId);
        Task<Result> AddOrUpdateProduct(Product product);
        Task<Result> Delete(int id);
    }
}
