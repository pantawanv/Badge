using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Badge.Data;
using Badge.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace Badge.Pages.Administration.User
{
    public class LeaderIndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration Configuration;

        public LeaderIndexModel(Badge.Data.ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        public string CurrentFilter { get; set; }
        public IList<ApplicationUser> User { get;set; } = default!;
        public List<ApplicationUser> SelectedUsers { get; set; }

        public async Task OnGetAsync(string? searchString)
        {
            if (SelectedUsers == null) { SelectedUsers = new List<ApplicationUser>(); }
            if (_context.Users != null)
            {
                IdentityRole role = await _context.Roles.FirstAsync(r => r.Name == "Leader");
                string roleid = role.Id;
                User = await (from u in _context.Users where (from r in _context.UserRoles where r.RoleId == roleid && r.UserId == u.Id select r).ToList().Count > 0 select u).ToListAsync();
                
                
                if (searchString != null)
                {
                    CurrentFilter = searchString;
                }

                if (User != null)
                {

                    if (CurrentFilter!= null)
                    {

                        IQueryable<ApplicationUser> UserIQ = from u in _context.Users where (from r in _context.UserRoles where r.RoleId == roleid && r.UserId == u.Id select r).ToList().Count > 0 select u;
                        UserIQ = UserIQ.Where(l => l.FName.Contains(searchString) || l.LName.Contains(searchString));


                        User =  UserIQ.ToList();
                    }
                }
            }
        }

        public async Task<IActionResult> SelectedUser (ApplicationUser user)
        {
            SelectedUsers.Add(user);
            LocalRedirect("./");
            Debug.WriteLine("yep");
            return RedirectToPage("./Index");
        }
    }
}
