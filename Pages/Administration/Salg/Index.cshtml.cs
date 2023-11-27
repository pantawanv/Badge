using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Badge.Data;
using Badge.Models;

namespace Badge.Pages.Administration.Salg
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Sale> Sale { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Sales != null)
            {
                Sale = await _context.Sales
                .Include(s => s.Channel)
                .Include(s => s.Seller)
                .Include(s => s.Ticket).ToListAsync();
            }
        }
    }
}
