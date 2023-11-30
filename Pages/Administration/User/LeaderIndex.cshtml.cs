using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Badge.Data;
using Badge.Models;
using Badge.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace Badge.Pages.Administration.User
{
    public class LeaderIndexModel : PageModel
    {
        private readonly Badge.Data.ApplicationDbContext _context;

        public LeaderIndexModel(Badge.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<ApplicationUser> User { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Users != null)
            {
                IdentityRole role = await _context.Roles.FirstAsync(r => r.Name == "Leader");
                string roleid = role.Id;
                User = await (from u in _context.Users where (from r in _context.UserRoles where r.RoleId == roleid && r.UserId == u.Id select r).ToList().Count > 0 select u).ToListAsync();
            }
        }
    }
}
