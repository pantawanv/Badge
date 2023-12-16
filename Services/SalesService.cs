using Badge.Data;
using Badge.Interfaces;

namespace Badge.Services
{
    public class SalesService : ISalesService
    {
        private readonly ApplicationDbContext _context;
        public SalesService(ApplicationDbContext context)
        {
            _context=context;
        }

        public int GetSalesCount()
        {
            return _context.Sales.Count();
        }

        public int GetTicketsCount()
        {
            return _context.Tickets.Count();
        }

    }
}
