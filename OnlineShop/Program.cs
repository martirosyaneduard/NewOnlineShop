using Microsoft.EntityFrameworkCore;
using OnlineShop.Data_Transfer_Object;
using OnlineShop.Models;
using OnlineShop.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .Enrich.FromLogContext()
  .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<OnlineShopDbContext>(options => 
                                       options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IService<Customer,CustomerDto>, CustomerService>();
builder.Services.AddScoped<IService<Order, OrderDto>, OrderService>();
builder.Services.AddScoped<IService<Product, ProductDto>, ProductService>();
builder.Services.AddScoped<IService<Category, CategoryDto>, CategoryService>();
builder.Services.AddScoped<ICustomerQuery, CustomerService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
