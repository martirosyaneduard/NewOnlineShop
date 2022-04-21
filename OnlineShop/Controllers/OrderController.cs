using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data_Transfer_Object;
using OnlineShop.Models;
using OnlineShop.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OnlineShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        readonly IService<Order, OrderDto>? _service;
        readonly ILogger<OrderController> _logger;
        public OrderController(IService<Order, OrderDto> service, ILogger<OrderController> logger)
        {
            this._service = service;
            this._logger = logger;
        }
 
        [HttpGet("GetAllOrders")]
        [ProducesResponseType(StatusCodes.Status200OK,
           Type = typeof(IAsyncEnumerable<Order>))]
        public IActionResult GetAllOrders()
        {
            var orders = _service.GetAll();
            return Ok(orders);
        }


        [HttpGet("{id}", Name = nameof(GetOrderById))]
        [ProducesResponseType(StatusCodes.Status200OK,
            Type = typeof(Order))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetOrderById(int id)
        {
            Order order;
            try
            {
                order = await _service.Get(id);
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
            return Ok(order);
        }

    
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(OrderDto orderDto)
        {
            Order order;
            try
            {
                order = await _service.Add(orderDto);
                _logger.LogInformation("Added new Order");
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

            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
        }

   
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id, OrderDto orderDto)
        {
            try
            {
                await _service.Update(orderDto, id);
                _logger.LogInformation("Updated, {id}", id);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.LogError(ex, "");
                return BadRequest();
            }
            catch (NullReferenceException ex)
            {
                _logger.LogError(ex, "");
                return BadRequest();
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
                _logger.LogError(ex,"");
                return BadRequest();
            }

            return NoContent();
        }
    }
}
