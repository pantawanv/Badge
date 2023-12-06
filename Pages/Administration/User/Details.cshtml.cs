using Badge.Areas.Identity.Data;
using Badge.Data;
using Badge.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Badge.Pages.Administration.User
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public ApplicationUser User { get; set; } = default!;
        public List<IdentityRole> Roles { get; set; } = default!;
        public IList<Group> Groups { get; set; } = default!;
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
            var roles = from r in _context.Roles where (from ur in _context.UserRoles where ur.UserId == id && ur.RoleId == r.Id select ur).Any() select r;
            Roles = await roles.ToListAsync();
            var groups = from g in _context.Groups where g.Leader.Id == id select g;
            Groups = await groups.Include(g => g.GroupType).Include(g => g.Members).ToListAsync();
            return Page();
        }
    }
}
