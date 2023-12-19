using Badge.Areas.Identity.Data;
using Badge.Data;
using Badge.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Badge.Pages.Admin.GroupAdmin
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            IdentityRole role = await _context.Roles.FirstAsync(r => r.Name == "Leader");
            string roleid = role.Id;
            var leaders = await (from u in _context.Users where (from r in _context.UserRoles where r.RoleId == roleid && r.UserId == u.Id select r).ToList().Count > 0 select u).ToListAsync();
            ViewData["GroupTypeId"] = new SelectList(_context.GroupTypes, "Id", "Name");
            ViewData["LeaderId"] = new SelectList(leaders, "Id", "FullName");
            return Page();
        }

        [BindProperty]
        public Group Group { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid || _context.Groups == null || Group == null)
            //  {
            //      return Page();
            //  }

            _context.Groups.Add(Group);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
