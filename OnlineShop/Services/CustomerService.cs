using OnlineShop.Models;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data_Transfer_Object;
using OnlineShop.Services.Validations;

namespace OnlineShop.Services
{
    public class CustomerService : IService<Customer, CustomerDto>,ICustomerQuery
    {
        readonly OnlineShopDbContext? _Context;
        public CustomerService(OnlineShopDbContext context)
        {
            this._Context = context;
        }
        public async Task<Customer> Add(CustomerDto entity)
        {
            Customer customer = new Customer()
            {
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                Phone = entity.Phone,
                Country = entity.Country,
            };
            _Context?.Customers?.Add(customer);
            await _Context.SaveChangesAsync();

            return customer;
        }

        public async Task Delete(int id)
        {
            CheckData.CheckPositiveNumber(id, "id");

            var customer = await _Context?.Customers?.FirstOrDefaultAsync(c => c.ID == id);
            if (customer is null)
            {
                throw new NullReferenceException();
            }
            _Context?.Customers?.Remove(customer);
            await _Context.SaveChangesAsync();
        }

        public async IAsyncEnumerable<Customer> GetAll()
        {
            var customers = _Context?.Customers.Include(o => o.Orders).ThenInclude(p => p.Products).AsNoTracking().AsAsyncEnumerable();
            await foreach (Customer customer in customers)
            {
                yield return customer;
            }
        }

        public async Task<Customer> Get(int id)
        {
            CheckData.CheckPositiveNumber(id, "id");

            Customer customer = await _Context.Customers.AsNoTracking().Include(o => o.Orders).ThenInclude(p => p.Products).FirstOrDefaultAsync(c => c.ID == id);
            if (customer is null)
            {
                throw new NullReferenceException();
            }
            return customer;
        }

        public async Task Update(CustomerDto entity, int id)
        {
            CheckData.CheckPositiveNumber(id, "id");

            Customer customer = await _Context.Customers.FirstOrDefaultAsync(c => c.ID == id);
            if (customer is null)
            {
                throw new NullReferenceException();
            }
            customer.FirstName = entity.FirstName;
            customer.LastName = entity.LastName;
            customer.Email = entity.Email;
            customer.Phone = entity.Phone;
            customer.Country = entity.Country;
            await _Context?.SaveChangesAsync();
        }

        public async  Task<string> GetMostOrdersName()
        {
            var name = _Context.Customers.Include(c => c.Orders).Select(c => new { c.FirstName, c.Orders.Count }).OrderByDescending(o => o.Count).First();
            return name.FirstName;
        }

    
    }
}
