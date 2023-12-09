using Badge.Areas.Identity.Data;
using Badge.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Badge.Pages.Administration.User
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        public IndexModel(ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
        public IList<ApplicationUser> Users { get; set; } = default!;

        public async Task OnGetAsync(string searchString, string? view)
        {
            string leaderId = _roleManager.Roles.First(r => r.Name == "Leader").Id;
            string managerId = _roleManager.Roles.First(r => r.Name == "Manager").Id;

            string View = view;
            if (_context.Users != null)
            {
                switch (View)
                {
                    case "leader":
                        Users = await (from u in _context.Users where _context.UserRoles.Where(r => r.UserId == u.Id && r.RoleId == leaderId).Any() select u).ToListAsync();
                        break;
                    case "manager":
                        Users = await (from u in _context.Users where _context.UserRoles.Where(r => r.UserId == u.Id && r.RoleId == managerId).Any() select u).ToListAsync();
                        break;
                    default:
                        Users = await (from u in _context.Users where _context.UserRoles.Where(r => r.UserId == u.Id && r.RoleId == leaderId).Any() || _context.UserRoles.Where(r=>r.UserId == u.Id && r.RoleId == managerId).Any() select u).ToListAsync();
                        break;
                }

                CurrentFilter = searchString;

                if (Users != null)
                {
                    if (CurrentFilter!= null)
                    {
                        Users = (from u in Users where u.FullName.ToUpper().Contains(CurrentFilter.ToUpper()) select u).ToList();
                    }
                }
            }
        }
    }
}
