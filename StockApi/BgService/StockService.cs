using Microsoft.EntityFrameworkCore;
using StockApi.Data;
using StockApi.Model;

namespace StockApi.BgService
{
    public class StockService:IStockService
    {
        private readonly AppDbContext _context;

        public StockService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Stock>> GetAll()
        {

            return await _context.Set<Stock>().AsNoTracking().ToListAsync();
        }
    }
}