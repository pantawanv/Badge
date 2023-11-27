using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Badge.Data;
using Badge.Models;

namespace Badge.Pages.Administration.Forældre
{
    public class IndexModel : PageModel
    {
        private readonly Badge.Data.BadgeContext _context;

        public IndexModel(Badge.Data.BadgeContext context)
        {
            _context = context;
        }

        public IList<Parent> Parent { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Parents != null)
            {
                Parent = await _context.Parents
                .Include(p => p.Member).ToListAsync();
            }
        }
    }
}
