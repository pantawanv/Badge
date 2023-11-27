using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Badge.Data;
using Badge.Models;

namespace Badge.Pages.Administration.Forældre
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
        ViewData["MemberId"] = new SelectList(_context.Members, "Id", "Id");
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
