using Badge.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Badge.Pages.Admin.Statistikker
{
    public class IndexModel : PageModel
    {

        private readonly ApplicationDbContext _context;
        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public int SalesTotal { get; set; }
        public int SalesWeek { get; set; }
        public int TicketsTotal { get; set; }

        public async Task OnGetAsync()
        {
            if (_context.Sales !=  null)
            {
                SalesTotal = _context.Sales.Count();

                var thisweek = from s in _context.Sales where s.SalesDate <= DateTime.Now && s.SalesDate >= (DateTime.Now.AddDays(-7)) select s;
                SalesWeek = thisweek.Count();
            }
            if (_context.Tickets != null)
            {
                TicketsTotal = _context.Tickets.Count();
            }

        }

    }

}
