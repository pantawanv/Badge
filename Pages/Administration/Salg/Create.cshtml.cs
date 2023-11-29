using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Badge.Data;
using Badge.Models;

namespace Badge.Pages.Administration.SalesAdmin
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
            _context.Sales.Add(Sale);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
