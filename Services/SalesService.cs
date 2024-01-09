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

        // Angiver den forventede indtjening per lod baseret på information fra https://kredsservice.fdf.dk/kredsadministration/fundraising/landslotteri
        public static double EstimatedEarningsPerTicket = 19.82;

        // Angiver slutdatoen
        public static DateTime endDate = new DateTime(2024, 1, 11, 12, 30, 0);

        // Henter slutdatoen for salget
        public DateTime GetEndDate()
        {
            return endDate;
        }

        // Henter alle salg
        public async Task<List<Sale>> GetAllSalesAsync()
        {
            return await _context.Sales.ToListAsync();
        }

        // Henter alle salg som en iqueryable 
        public IQueryable<Sale> GetAllSalesQuery()
        {
            return _context.Sales.AsQueryable();
        }

        // Henter antallet af salg som er lavet for hele kredsen
        public int GetSalesCount()
        {
            return _context.Sales.Count();
        }

        // Henter antallet af oprettede lodder
        public int GetTicketsCount()
        {
            return _context.Tickets.Count();
        }

        // Henter alle lodder
        public async Task<List<Ticket>> GetTicketsAsync()
        {
            return await _context.Tickets.Include(t => t.TicketGroupAssign).ThenInclude(t=>t.Group).Include(t => t.Sale).ToListAsync();
        }

        // Henter ikke tilgængelige lodder
        public async Task<List<Ticket>> GetAvailableTicketsAsync()
        {
            var tickets = _context.Tickets.Include(t => t.TicketGroupAssign).ThenInclude(t => t.Group).Include(t => t.Sale).Where(t => _context.Sales.Where(s => s.TicketId == t.Id).Any() == false);
            return await tickets.ToListAsync();
        }

        // Henter gruppe tildelingen for et lod der matcher det angivne id
        public async Task<Models.Group>? GetAssignedGroupAsync(string id)
        {
            var groupAssign = (await _context.TicketGroupAssigns.Include(t => t.Group).ThenInclude(t => t.GroupType).SingleOrDefaultAsync(t => t.TicketId == id));

            // Returnerer ingenting hvis der ikke er en gruppe tildeling 
            if (groupAssign == null)
            {
                return null;
            }
            return groupAssign.Group;
        }

        // Henter medlemstildelingen for et loder det matcher det angivne id
        public async Task<Member>? GetAssignedMemberAsync(string id)
        {
            var memberAssign = (await _context.TicketMemberAssigns.Include(t => t.Member).ThenInclude(t => t.User).SingleOrDefaultAsync(t => t.TicketId == id));

            // Returnerer ingenting hvis der ikke et medlemstildeling 
            if (memberAssign == null)
            {
                return null;
            }
            return memberAssign.Member;
        }


        // Henter alle salgskanaler 
        public List<Channel> GetChannels()
        {
            return _context.Channels.ToList();
        }

        // Opretter et salg
        public async Task AddSaleAsync(Sale saleToAdd)
        {
            _context.Sales.Add(saleToAdd);
            await _context.SaveChangesAsync();
            return;
        }

        // Henter den forventede indtjening 
        public double GetEstimatedTotalEarnings()
        {
            return EstimatedEarningsPerTicket * GetSalesCount();
        }

        // Henter alle salg lavet af medlemmet der matcher id'et 
        public async Task<List<Sale>> GetMembersSalesAsync(String id)
        {
            return await _context.Sales.Include(s => s.Channel).Where(s => s.SellerId == id).ToListAsync();
        }

        // Henter alle salg fra gruppen der matcher id'et
        public async Task<List<Sale>> GetGroupSalesAsync(int id)
        {
            return await _context.Sales.Include(s => s.Seller).ThenInclude(s=>s.Group).Where(s => s.Seller.Group.Id == id).ToListAsync();
        }

        // Henter en lodseddel der matcher id'et 
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

        // Group Assigns

        // Sletter en gruppetildeling 
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

        // Opretter en gruppe tildeling 
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

        // Finder en gruppetildeling der matcher id'et for en lodseddel
        public async Task<TicketGroupAssign> GetTicketGroupAssignAsync(string ticketid)
        {
            return await _context.TicketGroupAssigns.FirstOrDefaultAsync(t => t.TicketId == ticketid);
        }

        //Member Assigns

        // Sletter en medlemstildeling 
        public async Task DeleteTicketMemberAssignAsync(string id) 
        {
            TicketMemberAssign ticketMemberAssign = await _context.TicketMemberAssigns.FirstOrDefaultAsync(t => t.TicketId == id);
            if (ticketMemberAssign != null)
            {
                _context.TicketMemberAssigns.Remove(ticketMemberAssign);
                _context.SaveChanges();
            }
        }
        
        // Opretter en medlemstildeling  
        public async Task AddTicketMemberAssignAsync(string ticketid, string memberid)
        {
            TicketMemberAssign ticketMemberAssignToAdd = new TicketMemberAssign();
            ticketMemberAssignToAdd.TicketId = ticketid;
            ticketMemberAssignToAdd.MemberId = memberid;
            await _context.TicketMemberAssigns.AddAsync(ticketMemberAssignToAdd);
            _context.SaveChanges();
        }

        
        // Finder en medlemstildeling der matcher lodseddels id'et 
        public async Task<TicketMemberAssign> GetTicketMemberAssignAsync(string ticketid)
        {
            return await _context.TicketMemberAssigns.FirstOrDefaultAsync(t => t.TicketId == ticketid);
        }


        // Henter salget for en lodseddel
        public async Task<Sale> GetTicketSaleAsync(string ticketid)
        {
            return await _context.Sales.Include(t => t.Seller).ThenInclude(t => t.User).Include(t => t.Seller).ThenInclude(t => t.Group).Include(t=>t.Channel).FirstOrDefaultAsync(t => t.TicketId == ticketid);
        }
    }
}
