namespace OnlineShop.Services
{
    public interface ICustomerQuery
    {
       Task<string> GetMostOrdersName();
    }
}
