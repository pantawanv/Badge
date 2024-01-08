using Badge.Data;
using Badge.Interfaces;
using Badge.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Badge.Pages.Admin.ParentAdmin
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemberService _memberService;

        public EditModel(ApplicationDbContext context, IMemberService memberService)
        {
            _context = context;
            _memberService = memberService;
        }

        [BindProperty]
        public Parent Parent { get; set; } = default!;
        [BindProperty]
        public string SelectedMemberId { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id, string memberid)
        {
            SelectedMemberId = memberid;
            if (id == null || _memberService.GetParent(id) == null)
            {
                return NotFound();
            }

            var members = await _memberService.GetAllMembersAsync();
            ViewData["MemberId"] = new SelectList(members, "Id", "User.FullName");

            var parent = _memberService.GetParent(id);
            if (parent == null)
            {
                return NotFound();
            }
            Parent = parent;
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            _context.Attach(Parent).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_memberService.GetParent(Parent.Id) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }
        public IActionResult OnPostDeleteMemberParent(string memberid)
        {
            var memberParent = _context.MemberParents.FirstOrDefault(mp => mp.ParentId == Parent.Id && mp.MemberId == memberid);
            if (memberParent == null)
            {
                return NotFound();
            }
            else
            {
                _context.MemberParents.Remove(memberParent);
            }
            _context.SaveChanges();
            return RedirectToPage("Edit", new { id = Parent.Id });
        }

        public async Task<IActionResult> OnPostCreateMemberParentAsync(string memberid)
        {
            var memberparent = await _context.MemberParents.FirstOrDefaultAsync(m => m.MemberId == memberid && m.ParentId == Parent.Id);
            if (memberparent != null)
            {
                return RedirectToPage("Edit", new { id = Parent.Id });
            }
            else
            {
                MemberParent memberParentToAdd = new MemberParent() { MemberId = SelectedMemberId, ParentId = Parent.Id };
                await _context.MemberParents.AddAsync(memberParentToAdd);
                await _context.SaveChangesAsync();

                return RedirectToPage("Edit", new { id = Parent.Id });
            }
        }
    }
}
