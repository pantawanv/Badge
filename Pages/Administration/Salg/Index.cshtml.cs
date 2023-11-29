using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Badge.Data;
using Badge.Models;

namespace Badge.Pages.Administration.SalesAdmin
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public string CurrentFilter { get; set; }

        public IList<Sale> Sales{ get;set; } 

        public async Task OnGetAsync(string searchString)
        {
            CurrentFilter = searchString;

            IQueryable<Sale> salesIQ = from s in _context.Sales
                                        select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                salesIQ = salesIQ.Where(s => s.TicketId.Contains(searchString));
            }


            if (_context.Sales != null)
            {

                Sales = await salesIQ.AsNoTracking()
                .Include(s => s.Channel)
                .Include(s => s.Seller)
                .Include(s => s.Ticket).ToListAsync();
            }
        }
    }
}
