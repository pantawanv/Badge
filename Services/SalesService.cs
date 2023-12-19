using Badge.Data;
using Badge.Interfaces;
using Badge.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

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

        public IQueryable<Sale> GetAllSalesQuery()
        {
            return _context.Sales.AsQueryable();
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
            return await _context.Tickets.Include(t => t.TicketGroupAssign).ThenInclude(t=>t.Group).Include(t => t.Sale).ToListAsync();
        }

        public async Task<List<Ticket>> GetAvailableTicketsAsync()
        {
            var tickets = _context.Tickets.Include(t => t.TicketGroupAssign).ThenInclude(t => t.Group).Include(t => t.Sale).Where(t => _context.Sales.Where(s => s.TicketId == t.Id).Any() == false);
            return await tickets.ToListAsync();
        }

        public async Task<Models.Group>? GetAssignedGroupAsync(string id)
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

        //Get Ticket
        public async Task<Ticket>? GetTicketAsync(string id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return null;
            }
            else return ticket;
        }
        //Ticket Assigns
        //Delete Ticket Group Assign 
        public async Task DeleteTicketGroupAssignAsync(string id)
        {
            TicketGroupAssign ticketGroupAssign = await _context.TicketGroupAssigns.FirstOrDefaultAsync(t => t.TicketId == id);

            if (ticketGroupAssign != null)
            {
                _context.TicketGroupAssigns.Remove(ticketGroupAssign);
                _context.SaveChanges();

                await DeleteTicketMemberAssignAsync(id);
            }
        }
        //Delete Ticket Member Assign 
        public async Task DeleteTicketMemberAssignAsync(string id) 
        {
            TicketMemberAssign ticketMemberAssign = await _context.TicketMemberAssigns.FirstOrDefaultAsync(t => t.TicketId == id);
            if (ticketMemberAssign != null)
            {
                _context.TicketMemberAssigns.Remove(ticketMemberAssign);
                _context.SaveChanges();
            }
        }
        //Add Ticket Group Assign 
        public async Task AddTicketGroupAssignAsync(string ticketid, int groupid)
        {
            var ticketGroupAssign = await _context.TicketGroupAssigns.Include(t => t.Group).FirstOrDefaultAsync(t => t.TicketId == ticketid);
            if (ticketGroupAssign == null)
            {
                TicketGroupAssign ticketGroupAssignToAdd = new TicketGroupAssign();
                ticketGroupAssignToAdd.TicketId = ticketid;
                ticketGroupAssignToAdd.GroupId = groupid;
                await _context.TicketGroupAssigns.AddAsync(ticketGroupAssignToAdd);
                _context.SaveChanges();
            }
        }
        //Add Ticket Member Assign 
        public async Task AddTicketMemberAssignAsync(string ticketid, string memberid)
        {
            TicketMemberAssign ticketMemberAssignToAdd = new TicketMemberAssign();
            ticketMemberAssignToAdd.TicketId = ticketid;
            ticketMemberAssignToAdd.MemberId = memberid;
            await _context.TicketMemberAssigns.AddAsync(ticketMemberAssignToAdd);
            _context.SaveChanges();
        }

        //Get Ticket Group Assign
        public async Task<TicketGroupAssign> GetTicketGroupAssignAsync(string ticketid)
        {
            return await _context.TicketGroupAssigns.FirstOrDefaultAsync(t => t.TicketId == ticketid);
        }
        //Get Ticket Member Assign
        public async Task<TicketMemberAssign> GetTicketMemberAssignAsync(string ticketid)
        {
            return await _context.TicketMemberAssigns.FirstOrDefaultAsync(t => t.TicketId == ticketid);
        }
        //Get Ticket Sale
        public async Task<Sale> GetTicketSaleAsync(string ticketid)
        {
            return await _context.Sales.Include(t => t.Seller).ThenInclude(t => t.User).Include(t => t.Seller).ThenInclude(t => t.Group).Include(t=>t.Channel).FirstOrDefaultAsync(t => t.TicketId == ticketid);
        }
    }
}
