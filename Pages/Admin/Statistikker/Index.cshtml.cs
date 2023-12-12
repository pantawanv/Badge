using Badge.Data;
using Badge.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Badge.Pages.Admin.Statistikker
{
    public class IndexModel : PageModel
    {

        private readonly ApplicationDbContext _context;
        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Sales {get; set; }
        public int Tickets { get; set; }

        public async Task OnGetAsync()
        {
            if (_context.Sales !=  null) 
            {
                Sales = _context.Sales.Count();
            }
            if (_context.Tickets != null)
            {
                Tickets = _context.Tickets.Count();
            }
           
        }

    }

}
