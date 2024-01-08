using Badge.Data;
using Badge.Interfaces;
using Badge.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Badge.Pages.Admin.ParentAdmin
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemberService _memberService;

        public DetailsModel(ApplicationDbContext context,
            IMemberService memberService)
        {
            _context = context;
            _memberService = memberService;
        }

        public Parent Parent { get; set; } = default!;

        public IActionResult OnGet(int id)
        {
            if (id == null || _memberService.GetParent(id) == null)
            {
                return NotFound();
            }

            var parent = _memberService.GetParent(id);

            if (parent == null)
            {
                return NotFound();
            }
            else
            {
                Parent = parent;
            }
            return Page();
        }


    }
}
