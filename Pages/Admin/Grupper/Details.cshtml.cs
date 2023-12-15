using Badge.Data;
using Badge.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Badge.Pages.Admin.GroupAdmin
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Group Group { get; set; } = default!;
        public IList<Member> Members { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Groups == null)
            {
                return NotFound();
            }

            var group = await _context.Groups.Include(g => g.Leader).Include(g => g.GroupType).FirstOrDefaultAsync(m => m.Id == id);
            if (group == null)
            {
                return NotFound();
            }
            else
            {
                Group = group;
                var members = from m in _context.Members where m.GroupId == Group.Id select m;
                Members = await members.AsNoTracking().Include(m => m.Sales).Include(m => m.User).ToListAsync();

            }
            return Page();
        }
    }
}
