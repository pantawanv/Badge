using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Badge.Data;
using Badge.Models;

namespace Badge.Pages.Administration.Member
{
    public class DetailsModel : PageModel
    {
        private readonly Badge.Data.BadgeContext _context;

        public DetailsModel(Badge.Data.BadgeContext context)
        {
            _context = context;
        }

      public Models.Member Member { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Members == null)
            {
                return NotFound();
            }

            var member = await _context.Members.FirstOrDefaultAsync(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }
            else 
            {
                Member = member;
            }
            return Page();
        }
    }
}
