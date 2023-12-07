using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Badge.Data;
using Badge.Models;

namespace Badge.Pages.Administration.TicketAdmin
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration Configuration;


        public IndexModel(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        public string TicketSort { get; set; }
        public string GroupNameSort { get; set; }
        public string MemberNameSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public PaginatedList<Ticket> Tickets { get;set; } 
        public IQueryable<TicketGroupAssign> GroupAssigns { get; set; }
        public IQueryable<TicketMemberAssign> MemberAssigns { get; set; }
        public IQueryable<Sale> Sales { get; set; }

        public async Task OnGetAsync(string sortOrder,string searchString, int? pageIndex)
        {
            CurrentSort = sortOrder;

            TicketSort = String.IsNullOrEmpty(sortOrder) ? "ticket_desc" : "";
            GroupNameSort = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("group_asc") ? "group_desc" : "group_asc";
            MemberNameSort = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("member_asc") ? "member_desc" : "member_asc";


            if (searchString != null)
            {
                pageIndex = 1; 
            }
            else
            {
                searchString = CurrentFilter;
            }

            CurrentFilter = searchString;

            IQueryable<Ticket> ticketsIQ = from t in _context.Tickets
                                           select t;

            if(!String.IsNullOrEmpty(searchString)) 
            { 
                ticketsIQ = ticketsIQ.Where(t => t.Id.Contains(searchString) || GroupAssigns.FirstOrDefault(g => g.TicketId == t.Id).Group.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "ticket_desc":
                    ticketsIQ = ticketsIQ.OrderByDescending(t => t.Id);
                    break;
                case "group_desc":
                    ticketsIQ = ticketsIQ.OrderByDescending(t => GroupAssigns.FirstOrDefault(g => g.TicketId == t.Id).Group.Name);
                    break;
                case "group_asc":
                    ticketsIQ = ticketsIQ.OrderBy(t => GroupAssigns.FirstOrDefault(g => g.TicketId == t.Id).Group.Name);
                    break;
                case "member_desc":
                    ticketsIQ = ticketsIQ.OrderByDescending(t => MemberAssigns.FirstOrDefault(g => g.TicketId == t.Id).Member.FName);
                    break;
                case "member_asc":
                    ticketsIQ = ticketsIQ.OrderBy(t => MemberAssigns.FirstOrDefault(g => g.TicketId == t.Id).Member.FName);
                    break;
                default:
                    ticketsIQ = ticketsIQ.OrderBy(t => t.Id);
                    break;
            }

            var pageSize = Configuration.GetValue("PageSize", 4);
            var groupAssigns = from g in _context.TicketGroupAssigns select g;
            var memberAssigns = from m in _context.TicketMemberAssigns select m;
            var sales = from s in _context.Sales select s;
            GroupAssigns = groupAssigns.Include(g => g.Group).Include(g => g.Ticket);
            MemberAssigns = memberAssigns.Include(m => m.Member).Include(m => m.Ticket);
            Sales = sales.Include(s => s.Ticket);
            Tickets = await PaginatedList<Ticket>.CreateAsync(ticketsIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
          
        }
    }
}


