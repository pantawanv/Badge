using Badge.Data;
using Badge.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Badge.Pages.Administration.ParentAdmin
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
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "FName");
            return Page();
        }

        [BindProperty]
        public Parent Parent { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {

            _context.Parents.Add(Parent);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
