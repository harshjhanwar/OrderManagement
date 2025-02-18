using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderManagement.Models;
using OrderManagement.Repositories;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace OrderManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(IOrderRepository repository, ILogger<OrdersController> logger, AppDbContext context) : ControllerBase
    {
        private readonly IOrderRepository _repository = repository;
        private readonly ILogger<OrdersController> _logger = logger;
        private readonly AppDbContext _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Orders>>> GetOrders()
        {
            try
            {
                var orders = await _repository.GetAllAsync();
                return Ok(new { success = true, data = orders });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching orders");
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Orders order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Invalid data" });
            }

            try
            {
                var orderByInvoice = await _context.Orders.FirstOrDefaultAsync(x => x.Invoice == order.Invoice);
                if(orderByInvoice != null)
                {
                    return Ok(new { success = false, message = $"Order with invoice {order.Invoice} is already created." });
                }
                await _repository.AddAsync(order);
                return CreatedAtAction(nameof(GetOrders), new { id = order.Id }, new { success = true, data = order });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order");
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Orders order)
        {
            if (!ModelState.IsValid || id != order.Id)
            {
                return BadRequest(new { success = false, message = "Invalid data" });
            }

            try
            {
                await _repository.UpdateAsync(order);
                return Ok(new { success = true, data = order });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating order with ID {Id}", id);
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var order = await _repository.GetByIdAsync(id);
                if (order == null)
                {
                    return NotFound(new { success = false, message = "Order not found" });
                }

                bool val = await _repository.DeleteAsync(id);
                return val ? Ok(new { success = true, message = "Order deleted successfully" })
                           : StatusCode(500, new { success = false, message = "Error in delete operation." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting order with ID {Id}", id);
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }
    }
}
