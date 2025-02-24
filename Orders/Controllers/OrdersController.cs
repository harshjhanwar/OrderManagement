using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderManagement.Models;
using OrderManagement.OrderManagement.Business;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace OrderManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(IOrderRepository repository, ILogger<OrdersController> logger) : ControllerBase
    {
        private readonly IOrderRepository _repository = repository;
        private readonly ILogger<OrdersController> _logger = logger;
        //private readonly AppDbContext _context = context;

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
                Result result = await _repository.AddAsync(order);
                if (result.Success)
                    return Ok(new { success = true });

                return Ok(new { success = false, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in creating or updating the order");
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> Update(int id, [FromBody] Orders order)
        //{
        //    if (!ModelState.IsValid || id != order.Id)
        //    {
        //        return BadRequest(new { success = false, message = "Invalid data" });
        //    }

        //    try
        //    {
        //        Result result = await _repository.AddAsync(order);
        //        if (result.Success)
        //            return Ok(new { success = true });

        //        return Ok(new { success = false, message = result.Message });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error updating order with ID {Id}", id);
        //        return StatusCode(500, new { success = false, message = "Internal server error" });
        //    }
        //}

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

                Result result = await _repository.DeleteAsync(id);
                if (result.Success)
                    return Ok(new { success = true });

                return Ok(new { success = false, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting order with ID {Id}", id);
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }
    }
}
