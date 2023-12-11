using Badge.Data;
using Badge.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Badge.Pages.Administration.MemberAdmin
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Models.Member Member { get; set; } = default!;
        public IList<Parent> Parents { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Members == null || _context.Parents == null)
            {
                return NotFound();
            }
            var parents = from p in _context.Parents where p.MemberId == id select p;

            if (parents == null)
            {
                return NotFound();
            }
            else
            {
                Parents = await parents.ToListAsync();
            }

            var member = await _context.Members.Include(m => m.Group).FirstOrDefaultAsync(m => m.Id == id);
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
