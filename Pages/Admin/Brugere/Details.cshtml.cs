using Badge.Areas.Identity.Data;
using Badge.Data;
using Badge.Interfaces;
using Badge.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Badge.Pages.Admin.UserAdmin
{
    public class DetailsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IGroupService _groupService;
        private readonly ApplicationDbContext _context;

        public DetailsModel(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IGroupService groupService, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager=roleManager;
            _groupService=groupService;
            _context = context;
        }
        public ApplicationUser User { get; set; } = default!;
        public List<IdentityRole> Roles { get; set; } = default!;
        public List<Group> Groups { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(string? id)
        {

            if (id == null || _userManager.Users == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(id);
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
            
            var groups = from g in _groupService.GetGroups() where g.Leader.Id == id select g;
            Groups = await groups.Include(g => g.GroupType).Include(g => g.Members).ToListAsync();
            return Page();
        }
    }
}
