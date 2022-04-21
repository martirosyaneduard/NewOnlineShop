namespace OnlineShop.Services
{
    public interface IService<T,Dto>
    {
        IAsyncEnumerable<T> GetAll();
        Task<T> Get(int id);
        Task<T> Add(Dto entity);
        Task Update(Dto entity,int id);
        Task Delete(int id);
    }
}
