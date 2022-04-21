using Microsoft.EntityFrameworkCore;
using OnlineShop.Data_Transfer_Object;
using OnlineShop.Models;
using OnlineShop.Services.Validations;

namespace OnlineShop.Services
{
    public class OrderService : IService<Order, OrderDto>
    {
        readonly OnlineShopDbContext? _Context;
        public OrderService(OnlineShopDbContext context)
        {
            this._Context = context;
        }
        public async Task<Order> Add(OrderDto entity)
        {
            CheckData.CheckPositiveNumber(entity.CustomerId, nameof(entity.CustomerId));
            foreach(var productId in entity.ProductsId)
            {
                CheckData.CheckPositiveNumber(productId, "ProductId");
            }

            Order order = await CreatOrderAsync(entity);

            _Context?.Orders?.Add(order);
            await _Context.SaveChangesAsync();
            return order;
        }

        public async Task Delete(int id)
        {
            CheckData.CheckPositiveNumber(id, "id");

            var order = await _Context?.Orders?.FirstOrDefaultAsync(o => o.Id == id);
            if (order is null)
            {
                throw new NullReferenceException();
            }
            _Context?.Orders?.Remove(order);
            await _Context.SaveChangesAsync();
        }

        public async IAsyncEnumerable<Order> GetAll()
        {
            var orders = _Context?.Orders?.AsNoTracking().Include(o => o.Products).AsAsyncEnumerable();
            if (orders is null)
            {
                throw new NullReferenceException();
            }
            await foreach (Order order in orders)
            {
                yield return order;
            }
        }

        public async Task<Order> Get(int id)
        {
            CheckData.CheckPositiveNumber(id, "id");

            Order order = await _Context.Orders.AsNoTracking().Include(o=> o.Products).FirstOrDefaultAsync(o => o.Id == id);
            if (order is null)
            {
                throw new NullReferenceException();
            }
            return order;
        }

        public async Task Update(OrderDto entity, int id)
        {
            CheckData.CheckPositiveNumber(id, "id");
            CheckData.CheckPositiveNumber(entity.CustomerId, nameof(entity.CustomerId));
            foreach (var productId in entity.ProductsId)
            {
                CheckData.CheckPositiveNumber(productId, "ProductId");
            }

            Order order = await _Context.Orders.FirstOrDefaultAsync(o => o.Id == id);
            if (order is null)
            {
                throw new NullReferenceException();
            }
            Order ordernew = await CreatOrderAsync(entity);
            order.Status = ordernew.Status;
            order.Products = ordernew.Products;
            order.CustomerID = ordernew.CustomerID;
            _Context.SaveChanges();

        }
        private async Task<Order> CreatOrderAsync(OrderDto entity)
        {
            Customer customer = await _Context.Customers.FirstOrDefaultAsync(c => c.ID == entity.CustomerId);
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }
            List<Product> products = new List<Product>();
            foreach (var id in entity.ProductsId)
            {
                var product = await _Context.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (product != null)
                {
                    products.Add(product);
                }
            }
            if (products.Count == 0)
            {
                throw new InvalidOperationException();
            }
            Order order = new Order()
            {
                Status = entity.Status,
                CustomerID = entity.CustomerId,
                Products = products
            };
            return order;
        }

    }
}
