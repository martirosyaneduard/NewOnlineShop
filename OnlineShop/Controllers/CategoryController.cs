using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data_Transfer_Object;
using OnlineShop.Models;
using OnlineShop.Services;

namespace OnlineShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        readonly IService<Category, CategoryDto>? _service;
        readonly ILogger<CategoryController> _logger;
        public CategoryController(IService<Category, CategoryDto> service, ILogger<CategoryController> logger)
        {
            this._service = service;
            this._logger = logger;
        }

        [HttpGet("GetAllCategories")]
        [ProducesResponseType(StatusCodes.Status200OK,
            Type = typeof(IAsyncEnumerable<Category>))]
        public IActionResult GetAllCategory()
        {
            var categories = _service?.GetAll();

            return Ok(categories);
        }


        [HttpGet("{id}", Name = nameof(GetById))]
        [ProducesResponseType(StatusCodes.Status200OK,
            Type = typeof(Category))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById(int id)
        {
            Category category;
            try
            {
                category = await _service.Get(id);
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
            return Ok(category);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Post(CategoryDto categoryDto)
        {
            Category category;
            try
            {
                category = await _service.Add(categoryDto);
                _logger.LogInformation("Added new Category");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return BadRequest();
            }
            return CreatedAtAction(nameof(GetById), new {id = category.Id }, category);
        }


        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id, CategoryDto categoryDto)
        {
            try
            {
                await _service.Update(categoryDto, id);
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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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

            return NoContent();
        }
    }
}
