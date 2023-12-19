using Badge.Data;
using Badge.Interfaces;
using Badge.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Badge.Pages.Admin.GroupAdmin
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IGroupService _groupService;

        public DeleteModel(ApplicationDbContext context, IGroupService groupService)
        {
            _context = context;
            _groupService = groupService;
        }

        [BindProperty]
        public Group Group { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null || _context.Groups == null)
            {
                return NotFound();
            }

            var group = await _groupService.GetGroupAsync(id);

            if (group == null)
            {
                return NotFound();
            }
            else
            {
                Group = group;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (id == null || _groupService.GetGroups() == null)
            {
                return NotFound();
            }
            var group = await _groupService.GetGroupAsync(id);

            if (group != null)
            {
                await _groupService.DeleteGroupAsync(group);
            }

            return RedirectToPage("./Index");
        }
    }
}
