using Badge.Areas.Identity.Data;
using Badge.Data;
using Badge.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Badge.Pages.Admin.MemberAdmin
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMemberService _memberService;

        public DeleteModel(UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            IMemberService memberService)
        {
            _userManager = userManager;
            _context = context;
            _memberService = memberService;
        }

        [BindProperty]
        public Models.Member Member { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (id == null || _context.Members == null)
            {
                return NotFound();
            }

            var member = await _context.Members.Include(m => m.User).FirstOrDefaultAsync(m => m.Id == id);

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

        public async Task<IActionResult> OnPostAsync(string? id)
        {
            var member = await _memberService.GetMemberAsync(id);
            await _memberService.DeleteMemberAsync(member);
            return RedirectToPage("./Index");
        }
    }
}
