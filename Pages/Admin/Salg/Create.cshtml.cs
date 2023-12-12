using Badge.Data;
using Badge.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Badge.Pages.Admin.SalesAdmin
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
            var members = _context.Members.Include(m => m.User).ToList();
            ViewData["ChannelId"] = new SelectList(_context.Channels, "Id", "Name");
            ViewData["SellerId"] = new SelectList(members, "Id", "User.FullName");
            ViewData["TicketId"] = new SelectList(from t in _context.Tickets where (from s in _context.Sales where s.TicketId == t.Id select s).Any() == false select t, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public Sale Sale { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            _context.Sales.Add(Sale);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
