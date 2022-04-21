using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data_Transfer_Object;
using OnlineShop.Models;
using OnlineShop.Services;

namespace OnlineShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        readonly IService<Product, ProductDto>? _service;
        readonly ILogger<ProductController> _logger;
        public ProductController(IService<Product, ProductDto> service, ILogger<ProductController> logger)
        {
            this._service = service;
            this._logger = logger;
        }


        [HttpGet("GetAllProducts")]
        [ProducesResponseType(StatusCodes.Status200OK,
           Type = typeof(IAsyncEnumerable<Product>))]
        public async Task<IActionResult> GetAllProduct()
        {
            var products = _service.GetAll();
            return Ok(products);
        }

        [HttpGet("{id}", Name = nameof(GetProductById))]
        [ProducesResponseType(StatusCodes.Status200OK,
            Type = typeof(Product))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetProductById(int id)
        {
            Product product;
            try
            {
                product = await _service.Get(id);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.LogError(ex, "");
                return BadRequest();
            }
            catch (NullReferenceException ex)
            {
                _logger.LogError(ex,"");
                return NotFound();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "");
                return BadRequest();  
            }
            return Ok(product);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(ProductDto productDto)
        {
            Product product;
            try
            {
                product = await _service.Add(productDto);
                _logger.LogInformation("Added new Product");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.LogError(ex, "");
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }


        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id, ProductDto productDto)
        {
            try
            {
                await _service.Update(productDto, id);
                _logger.LogInformation("Updated, {id}", id);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.LogError(ex, "");
                return BadRequest();
            }
            catch (NullReferenceException ex)
            {
                _logger.LogError(ex,"");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return BadRequest();
            }
            return NoContent();
        }

        
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.Delete(id);
                _logger.LogInformation("Deleted, {id}", id);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.LogError(ex, "");
                return BadRequest();
            }
            catch (NullReferenceException ex)
            {
                _logger.LogError(ex,"");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return BadRequest();
            }

            return NoContent();
        }
    }
}
