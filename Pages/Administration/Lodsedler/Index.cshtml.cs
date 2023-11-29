using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Badge.Data;
using Badge.Models;

namespace Badge.Pages.Administration.Lodsedler
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public string CurrentFilter { get; set; }

        public IList<Ticket> Tickets { get;set; } 

        public async Task OnGetAsync(string searchString)
        {

            CurrentFilter = searchString;

            IQueryable<Ticket> ticketsIQ = from t in _context.Tickets
                                           select t;

            if(!String.IsNullOrEmpty(searchString)) 
            { 
                ticketsIQ = ticketsIQ.Where(t => t.Id.Contains(searchString));
            }


            if (_context.Tickets != null)
            {
               Tickets = await ticketsIQ.AsNoTracking().ToListAsync();
                    
            }
        }


    
        





    }
}


