using Badge.Areas.Identity.Data;
using Badge.Data;
using Badge.Interfaces;
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
        private readonly IGroupService _groupService;

        public CreateModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IGroupService groupService)
        {
            _context = context;
            _userManager = userManager;
            _groupService = groupService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var leaders = (await _userManager.GetUsersInRoleAsync("Leader"));
            ViewData["GroupTypeId"] = new SelectList(await _context.GroupTypes.ToListAsync(), "Id", "Name");
            ViewData["LeaderId"] = new SelectList(leaders.ToList(), "Id", "FullName");
            return Page();
        }

        [BindProperty]
        public Group Group { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            await _groupService.CreateGroupAsync(Group);

            return RedirectToPage("./Index");
        }
    }
}
