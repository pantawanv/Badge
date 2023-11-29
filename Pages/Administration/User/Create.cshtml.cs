using Badge.Areas.Identity.Data;
using Badge.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Badge.Pages.Administration.User
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult OnGet()
        {
           return Page();
        }

        [BindProperty]
        public ApplicationUser User { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            _context.Users.Add(User);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }
    }
}
