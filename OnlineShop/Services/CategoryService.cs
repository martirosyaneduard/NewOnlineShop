using Microsoft.EntityFrameworkCore;
using OnlineShop.Data_Transfer_Object;
using OnlineShop.Models;
using OnlineShop.Services.Validations;

namespace OnlineShop.Services
{
    public class CategoryService : IService<Category, CategoryDto>
    {
        readonly OnlineShopDbContext? _Context;
        public CategoryService(OnlineShopDbContext context)
        {
            this._Context = context;
        }
        public async Task<Category> Add(CategoryDto entity)
        {
            Category category = new Category()
            {
                Name = entity.Name,
            };
            _Context?.Categories?.Add(category);
            await _Context.SaveChangesAsync();
            return category;
        }

        public async Task Delete(int id)
        {
            CheckData.CheckPositiveNumber(id, "id");

            var category = await _Context?.Categories?.FirstOrDefaultAsync(c => c.Id == id);
            if (category is null)
            {
                throw new NullReferenceException();
            }
            _Context?.Categories?.Remove(category);
            await _Context.SaveChangesAsync();
        }

        public async IAsyncEnumerable<Category> GetAll()
        {
            var categorys = _Context?.Categories.Include(p => p.Products).AsNoTracking().AsAsyncEnumerable();

            await foreach (Category category in categorys)
            {
                yield return category;
            }
        }

        public async Task<Category> Get(int id)
        {
            CheckData.CheckPositiveNumber(id, "id");

            Category category = await _Context.Categories.AsNoTracking().Include(p => p.Products).FirstOrDefaultAsync(c => c.Id == id);
            if (category is null)
            {
                throw new NullReferenceException();
            }
            return category;
        }

        public async Task Update(CategoryDto entity, int id)
        {
            CheckData.CheckPositiveNumber(id, "id");

            Category category = await _Context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category is null)
            {
                throw new NullReferenceException();
            }
            category.Name = entity.Name;
            await _Context?.SaveChangesAsync();
        }
    }
}
