using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderManagement.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderManagement.Repositories
{
    public class OrderRepository(AppDbContext context, ILogger<OrderRepository> logger) : IOrderRepository
    {
        private readonly AppDbContext _context = context;
        private readonly ILogger<OrderRepository> _logger = logger;

        public async Task<IEnumerable<Orders>> GetAllAsync()
        {
            try
            {
                return await _context.Orders.AsNoTracking().ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<Orders?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Orders.FindAsync(id);
            }
            catch
            {
                throw;
            }
        }

        public async Task AddAsync(Orders order)
        {
            try
            {
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task UpdateAsync(Orders order)
        {
            try
            {
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
            }
            catch 
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var order = await _context.Orders.FindAsync(id);
                if (order != null)
                {
                    var rowsAffectedParam = new SqlParameter("@RowsAffected", System.Data.SqlDbType.SmallInt) { Direction = System.Data.ParameterDirection.Output };
                    await _context.Database.ExecuteSqlInterpolatedAsync(
                        $"dbo.spDeleteOrder_Orders @id = {id}, @RowsAffected = {rowsAffectedParam} OUTPUT"
                    );
                    return (short)rowsAffectedParam.Value == 1;
                }
                return false;
            }
            catch
            {
                throw;
            }
        }
    }
}
