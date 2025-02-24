using Dapper;
using Microsoft.Data.SqlClient;
using OrderManagement.Models;
using OrderManagement.OrderManagement.Business;
using System.Data;

namespace OrderManagement.OrderManagement.DataAccess
{
    public class ProductsSPRepository(IConfiguration configuration, ILogger<ProductsSPRepository> logger) : IProductRepository
    {
        private readonly string _connectionString = configuration.GetConnectionString("SQLServerConnectionString")!;
        private readonly ILogger<ProductsSPRepository> _logger = logger;
        public async Task<Result> AddOrUpdateProduct(Product product)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();

                parameters.Add("@criteria", "Insert_or_update_product", DbType.String);
                parameters.Add("@id", product.ProductId, DbType.Int32);
                parameters.Add("@ProductName", product.ProductName, DbType.String);
                parameters.Add("@UnitPrice", product.UnitPrice, DbType.Decimal);
                parameters.Add("@NoOfUnits", product.NoOfUnits, DbType.Int32);
                parameters.Add("@Discount", product.Discount, DbType.Decimal);
                parameters.Add("@OrderId", product.OrderId, DbType.Int32);
                parameters.Add("@RowsAffected", dbType: DbType.Int16, direction: ParameterDirection.Output);

                await connection.ExecuteAsync("spProductsDataDML_Orders", parameters, commandType: CommandType.StoredProcedure);
                short rowsAffected = parameters.Get<short>("@RowsAffected");

                if (rowsAffected == 1)
                {
                    return new Result(true);
                }
                else
                {
                    return new Result(false, "An error occurred while inserting or updating the product. Please try again later.");
                }
            }
            catch(Exception ex) {
                _logger.LogError(ex, "Error in creating or updating product");
                return new Result(false, "An error occurred while inserting or updating the product. Please try again later.");
                
            }
        }

        public async Task<Result> Delete(int id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();

                parameters.Add("@criteria", "Delete_product", DbType.String);
                parameters.Add("@id", id, DbType.Int32);
                parameters.Add("@RowsAffected", dbType: DbType.Int16, direction: ParameterDirection.Output);

                await connection.ExecuteAsync("spProductsDataDML_Orders", parameters, commandType: CommandType.StoredProcedure);

                short rowsAffected = parameters.Get<short>("@RowsAffected");

                if (rowsAffected == 1)
                {
                    return new Result(true);
                }
                else
                {
                    return new Result(false, "An error occurred while deleting the product. Please try again later.");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in deleting the product");
                return new Result(false, "An error occurred while deleting the product. Please try again later.");
            }

        }

        public async Task<IEnumerable<Product>> GetAllProducts(int orderId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@orderId", orderId, DbType.Int32);

                var products = await connection.QueryAsync<Product>("spGetProducts_Orders", parameters, commandType: CommandType.StoredProcedure);
                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in fetching products");
                return [];

            }
        }
    }
}
