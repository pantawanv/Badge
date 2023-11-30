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
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public PaginatedList<Ticket> Tickets { get;set; } 

        public async Task OnGetAsync(string sortOrder,string searchString, int? pageIndex)
        {
            CurrentSort = sortOrder;

            TicketSort = String.IsNullOrEmpty(sortOrder) ? "ticket_desc" : "";

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
                ticketsIQ = ticketsIQ.Where(t => t.Id.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "ticket_desc":
                    ticketsIQ = ticketsIQ.OrderByDescending(t => t.Id);
                    break;
             
            }

            var pageSize = Configuration.GetValue("PageSize", 4);
            Tickets = await PaginatedList<Ticket>.CreateAsync(ticketsIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
          
        }


    
        





    }
}


