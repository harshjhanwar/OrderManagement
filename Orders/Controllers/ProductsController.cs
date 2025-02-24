using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using OrderManagement.Models;
using OrderManagement.OrderManagement.Business;
using System.Data;
using System.Runtime.CompilerServices;

namespace OrderManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductRepository productRepository, ILogger<ProductsController> logger) : ControllerBase
    {
        private readonly IProductRepository _productRepository = productRepository;
        private readonly ILogger<ProductsController> _logger = logger;
        //private readonly string connectionString = configuration.GetConnectionString("SQLServerConnectionString");

        [HttpGet("{orderId}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(int orderId)
        {
            try
            {
                var products = await _productRepository.GetAllProducts(orderId);
                return Ok(new { success = true, data = products });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching products");
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)
        {
            try
            {
                Result result = await _productRepository.AddOrUpdateProduct(product);
                if (result.Success)
                    return Ok(new { success = true });

                return Ok(new { success = false, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in creating or updating the product");
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            try
            {
                Result result = await _productRepository.Delete(productId);
                if (result.Success)
                    return Ok(new { success = true });

                return Ok(new { success = false, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in deleting the product with productId {productId}", productId);
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }
    }
}
