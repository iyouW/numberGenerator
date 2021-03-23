namespace numberPool.App.Services.NumberPool
{
    using System.Threading.Tasks;

    public interface INumberPoolService
    {
        Task<long> RentAsync();
        Task ReturnAsync(long number);
    }
}