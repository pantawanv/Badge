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
    public class DeleteModel : PageModel
    {
        private readonly Badge.Data.BadgeContext _context;

        public DeleteModel(Badge.Data.BadgeContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Parent Parent { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Parents == null)
            {
                return NotFound();
            }

            var parent = await _context.Parents.FirstOrDefaultAsync(m => m.Id == id);

            if (parent == null)
            {
                return NotFound();
            }
            else 
            {
                Parent = parent;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Parents == null)
            {
                return NotFound();
            }
            var parent = await _context.Parents.FindAsync(id);

            if (parent != null)
            {
                Parent = parent;
                _context.Parents.Remove(Parent);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
