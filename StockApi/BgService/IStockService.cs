using StockApi.Model;

namespace StockApi.BgService
{
    public interface IStockService
    {
        Task<IEnumerable<Stock>>GetAll();
    }
}
