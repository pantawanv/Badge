using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Badge.Data;
using Badge.Models;
using Badge.Areas.Identity.Data;
using System.Drawing.Printing;

namespace Badge.Pages.Administration.GroupAdmin
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

      public Group Group { get; set; } = default!;
      public IList<Member> Members { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Groups == null)
            {
                return NotFound();
            }

            var group = await _context.Groups.FirstOrDefaultAsync(m => m.Id == id);
            if (group == null)
            {
                return NotFound();
            }
            else 
            {
                Group = group;
                var members = from m in _context.Members where m.GroupId == Group.Id select m;
                Members = await members.AsNoTracking().Include(m => m.Sales).ToListAsync();

            }
            return Page();
        }
    }
}
