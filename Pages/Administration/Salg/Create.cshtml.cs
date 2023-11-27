using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Badge.Data;
using Badge.Models;

namespace Badge.Pages.Administration.Salg
{
    public class CreateModel : PageModel
    {
        private readonly Badge.Data.BadgeContext _context;

        public CreateModel(Badge.Data.BadgeContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["ChannelId"] = new SelectList(_context.Channels, "Id", "Name");
        ViewData["SellerId"] = new SelectList(_context.Members, "Id", "Id");
        ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public Sale Sale { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Sales == null || Sale == null)
            {
                return Page();
            }

            _context.Sales.Add(Sale);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
