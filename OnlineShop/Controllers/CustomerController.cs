using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data_Transfer_Object;
using OnlineShop.Models;
using OnlineShop.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OnlineShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        readonly ICustomerQuery _customerQuery;
        readonly IService<Customer, CustomerDto>? _service;
        readonly ILogger<CustomerController> _logger;
        public CustomerController(IService<Customer, CustomerDto> service, ICustomerQuery customerQuery, ILogger<CustomerController> logger)
        {
            this._service = service;
            this._customerQuery = customerQuery;
            this._logger = logger;
        }

        [HttpGet("GetAllCustomers")]
        [ProducesResponseType(StatusCodes.Status200OK,
            Type = typeof(IAsyncEnumerable<Customer>))]
        public IActionResult GetAllCustomers()
        {
            var customers = _service.GetAll();
            return Ok(customers);
        }


        [HttpGet("{id}", Name = nameof(GetCustomerById))]
        [ProducesResponseType(StatusCodes.Status200OK,
            Type = typeof(Customer))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            Customer customer;
            try
            {
                customer = await _service.Get(id);
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
            return Ok(customer);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Post(CustomerDto customerDto)
        {
            Customer customer;
            try
            {
                customer = await _service.Add(customerDto);
                _logger.LogInformation("Added new Customer");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return BadRequest();
            }
            return CreatedAtAction(nameof(GetCustomerById), new { id = customer.ID }, customer);
        }


        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id, CustomerDto customerDto)
        {

            try
            {
                await _service.Update(customerDto, id);
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
                return NotFound();
            }
            catch(Exception ex)
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
                _logger.LogError(ex.Message);
                return NotFound();
            }
            catch (Exception ex)
            { 
                _logger.LogError(ex, "");
                return BadRequest();
            }

            return NoContent();
        }

        [HttpGet("Name")]
        [ProducesResponseType(StatusCodes.Status200OK,
            Type = typeof(string))]
        public async Task<IActionResult> GetCustomerName()
        {
            string name;
            try
            {
                name = await _customerQuery.GetMostOrdersName();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "");
                return BadRequest();
            }
            return Ok(name);
        }
    }
}
