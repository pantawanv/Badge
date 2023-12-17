using Badge.Data;
using Badge.Interfaces;
using Badge.Models;
using Microsoft.EntityFrameworkCore;

namespace Badge.Services
{
    public class SalesService : ISalesService
    {
        private readonly ApplicationDbContext _context;
        public SalesService(ApplicationDbContext context)
        {
            _context=context;
        }

        public static double EstimatedEarningsPerTicket = 19.82;

        public async Task<List<Sale>> GetAllSalesAsync()
        {
            return await _context.Sales.ToListAsync();
        }

        public int GetSalesCount()
        {
            return _context.Sales.Count();
        }

        public int GetTicketsCount()
        {
            return _context.Tickets.Count();
        }

        public async Task<List<Ticket>> GetTicketsAsync()
        {
            return await _context.Tickets.ToListAsync();
        }

        public async Task<List<Ticket>> GetAvailableTicketsAsync()
        {
            var tickets = from t in _context.Tickets where !(from s in _context.Sales where s.TicketId == t.Id select s).Any() select t;
            return await tickets.ToListAsync();
        }

        public async Task<Group>? GetAssignedGroupAsync(string id)
        {
            var groupAssign = (await _context.TicketGroupAssigns.Include(t => t.Group).ThenInclude(t => t.GroupType).SingleOrDefaultAsync(t => t.TicketId == id));
            if (groupAssign == null)
            {
                return null;
            }
            return groupAssign.Group;
        }

        public async Task<Member>? GetAssignedMemberAsync(string id)
        {
            var memberAssign = (await _context.TicketMemberAssigns.Include(t => t.Member).ThenInclude(t => t.User).SingleOrDefaultAsync(t => t.TicketId == id));
            if (memberAssign == null)
            {
                return null;
            }
            return memberAssign.Member;
        }

        public async Task<Ticket>? GetTicketAsync(string id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return null;
            }
            else return ticket;
        }

        public List<Channel> GetChannels()
        {
            return _context.Channels.ToList();
        }

        public async Task AddSaleAsync(Sale saleToAdd)
        {
            _context.Sales.Add(saleToAdd);
            await _context.SaveChangesAsync();
            return;
        }

        public double GetEstimatedTotalEarnings()
        {
            return EstimatedEarningsPerTicket * GetSalesCount();
        }

        public async Task<List<Sale>> GetMembersSalesAsync(String id)
        {
            return await _context.Sales.Include(s => s.Channel).Where(s => s.SellerId == id).ToListAsync();
        }
        public async Task<List<Sale>> GetGroupSalesAsync(int id)
        {
            return await _context.Sales.Include(s => s.Seller).ThenInclude(s=>s.Group).Where(s => s.Seller.Group.Id == id).ToListAsync();
        }
    }
}
