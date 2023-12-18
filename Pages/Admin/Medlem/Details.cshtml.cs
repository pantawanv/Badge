using Badge.Data;
using Badge.Interfaces;
using Badge.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Badge.Pages.Admin.MemberAdmin
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemberService _memberService;

        public DetailsModel(ApplicationDbContext context, IMemberService memberService)
        {
            _context = context;
            _memberService = memberService;
        }

        public Member Member { get; set; } = default!;
        public IList<Parent> Parents { get; set; }

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (id == null || _context.Members == null || _context.Parents == null)
            {
                return NotFound();
            }
            var parents = await _memberService.GetParentsOfMemberAsync(id);

            if (parents == null)
            {
                return NotFound();
            }
            else
            {
                Parents = parents;
            }

            var member = await _context.Members.Include(m => m.Group).Include(m => m.User).FirstOrDefaultAsync(m => m.Id == id);
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
    }
}
