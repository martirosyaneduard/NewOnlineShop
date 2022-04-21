using Microsoft.EntityFrameworkCore;
using OnlineShop.Data_Transfer_Object;
using OnlineShop.Models;
using OnlineShop.Services.Validations;

namespace OnlineShop.Services
{
    public class ProductService : IService<Product, ProductDto>
    {
        readonly OnlineShopDbContext? _Context;
        public ProductService(OnlineShopDbContext context)
        {
            this._Context = context;
        }
        public async Task<Product> Add(ProductDto entity)
        {
            CheckData.CheckPositiveNumber(entity.Price, nameof(entity.Price));
            CheckData.CheckPositiveNumber(entity.Weight, nameof(entity.Weight));
            CheckData.CheckPositiveNumber(entity.CategoryID, nameof(entity.CategoryID));

            Product product = new Product()
            {
                Name = entity.Name,
                Price = entity.Price,
                Weight = entity.Weight,
                Description = entity.Description,
                CategoryID = entity.CategoryID,
            };
            _Context?.Products?.Add(product);
            await _Context.SaveChangesAsync();
            return product;
        }

        public async Task Delete(int id)
        {
            CheckData.CheckPositiveNumber(id, "id");

            var product = await _Context?.Products?.FirstOrDefaultAsync(c => c.Id == id);
            if (product is null)
            {
                throw new NullReferenceException();
            }
            _Context?.Products?.Remove(product);
            await _Context.SaveChangesAsync();
        }

        public async IAsyncEnumerable<Product> GetAll()
        {
            var products = _Context?.Products;
            if (products is null)
            {
                throw new NullReferenceException();
            }
            await foreach (Product product in products)
            {
                yield return product;
            }
        }

        public async Task<Product> Get(int id)
        {
            CheckData.CheckPositiveNumber(id, "id");

            Product product = await _Context.Products.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (product is null)
            {
                throw new NullReferenceException();
            }
            return product;
        }

        public async Task Update(ProductDto entity, int id)
        {
            CheckData.CheckPositiveNumber(id, "id");
            CheckData.CheckPositiveNumber(entity.Price, nameof(entity.Price));
            CheckData.CheckPositiveNumber(entity.Weight, nameof(entity.Weight));
            CheckData.CheckPositiveNumber(entity.CategoryID, nameof(entity.CategoryID));

            Product product = await _Context.Products.FirstOrDefaultAsync(c => c.Id == id);
            if (product is null)
            {
                throw new NullReferenceException();
            }
            product.Name = entity.Name;
            product.Price = entity.Price;
            product.Weight = entity.Weight;
            product.Description = entity.Description;
            product.CategoryID = entity.CategoryID;
            await _Context?.SaveChangesAsync();
        }
    }
}
