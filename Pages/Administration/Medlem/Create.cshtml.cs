using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Badge.Data;
using Badge.Models;
using System.Diagnostics;

namespace Badge.Pages.Administration.Member
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
        ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Models.Member Member { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            Debug.WriteLine(Member.GroupId);
            
            var group = from g in _context.Groups where g.Id == Member.GroupId select g;
            if (group!=null)
            {
                Member.Group = group as Group;
            }

            _context.Members.Add(Member);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
