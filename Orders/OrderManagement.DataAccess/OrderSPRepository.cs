using Dapper;
using Microsoft.Data.SqlClient;
using OrderManagement.Models;
using OrderManagement.OrderManagement.Business;
using System.Data;
using System.Runtime.CompilerServices;

namespace OrderManagement.OrderManagement.DataAccess
{
    public class OrderSPRepository(IConfiguration configuration, ILogger<OrderSPRepository> logger) : IOrderRepository
    {
        private readonly string _connectionString = configuration.GetConnectionString("SQLServerConnectionString")!;
        private readonly ILogger<OrderSPRepository> _logger = logger;
        public async Task<Result> AddAsync(Orders order)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();

                parameters.Add("@criteria", "Insert_or_update_order", DbType.String);
                parameters.Add("@id", order.Id, DbType.Int32);
                parameters.Add("@OrderDeliveryStatus", order.OrderDeliveryStatus, DbType.Int32);
                parameters.Add("@OrderStatus", order.OrderStatus, DbType.Int32);
                parameters.Add("@Invoice", order.Invoice, DbType.Int32);
                parameters.Add("@OrderDate", order.OrderDate, DbType.DateTime2);
                parameters.Add("@ShippedDate", order.ShippedDate, DbType.DateTime2);
                parameters.Add("@Company", order.Company, DbType.String);
                parameters.Add("@Store", order.Store, DbType.String);
                parameters.Add("@OrderTotal", order.OrderTotal, DbType.Decimal);
                parameters.Add("@PaymentTotal", order.PaymentTotal, DbType.Decimal);
                parameters.Add("@RowsAffected", dbType: DbType.Int16, direction: ParameterDirection.Output);

                await connection.ExecuteAsync("spOrdersDataDML_Orders", parameters, commandType: CommandType.StoredProcedure);
                short rowsAffected = parameters.Get<short>("@RowsAffected");

                if(rowsAffected == 1)
                {
                    return new Result(true);
                }
                else
                {
                    if (rowsAffected == -3)
                    {
                        return new Result(false, $"Order with invoice {order.Invoice} already present.");
                    }
                    return new Result(false, "An error occurred while inserting or updating the order. Please try again later.");
                }

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in creating or updating orders");
                return new Result(false, "An error occurred while inserting or updating the order. Please try again later.");
            }

            
        }

        public async Task<Result> DeleteAsync(int id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@criteria", "Delete_record", DbType.String);
                parameters.Add("@id", id, DbType.Int32);
                parameters.Add("@RowsAffected", dbType: DbType.Int16, direction: ParameterDirection.Output);

                await connection.ExecuteAsync("spOrdersDataDML_Orders", parameters, commandType: CommandType.StoredProcedure);

                short rowsAffected = parameters.Get<short>("@RowsAffected");

                if (rowsAffected == 1)
                {
                    return new Result(true);
                }
                else
                {
                    return new Result(false, "An error occurred while deleting the order. Please try again later.");
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in deleting orders");
                return new Result(false, "An error occurred while deleting the order. Please try again later.");
            }
        }

        public async Task<IEnumerable<Orders>> GetAllAsync()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var orders = await connection.QueryAsync<Orders>("spGetOrders_Orders", commandType: CommandType.StoredProcedure);
                return orders.ToList();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in fetching order");
                return [];
            }
        }

        public async Task<Orders?> GetByIdAsync(int id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@id", id, DbType.Int32);
                var orders = await connection.QueryAsync<Orders>("spGetOrders_Orders", commandType: CommandType.StoredProcedure);
                return orders.FirstOrDefault();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in fetching the order by id: {id}", id);
                return null;
            }
        }
    }
}
