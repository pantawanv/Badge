using Badge.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Badge.Pages.Administration.User
{
    public class ManagerDetailsModel : PageModel
    {
        private readonly Badge.Data.ApplicationDbContext _context;

        public ManagerDetailsModel(Badge.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public ApplicationUser User { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                User = user;
            }
            return Page();
        }
    }
}
