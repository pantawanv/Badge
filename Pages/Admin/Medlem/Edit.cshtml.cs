using Badge.Areas.Identity.Data;
using Badge.Data;
using Badge.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Badge.Pages.Admin.MemberAdmin
{
    public class EditModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public EditModel(UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Fornavn")]
            public string FName { get; set; }
            [Display(Name = "Efternavn")]
            public string LName { get; set; }
            [EmailAddress]
            [Display(Name = "Emailadresse")]
            public string Email { get; set; }
            [Display(Name = "Gruppe")]
            public int? GroupId { get; set; }
        }

        private async Task LoadAsync(Member member)
        {
            var fName = member.User.FName;
            var lName = member.User.LName;
            var email = member.User.Email;
            var groupId = member.GroupId;

            Input = new InputModel
            {
                FName = fName,
                LName = lName,
                Email = email,
                GroupId = groupId
            };
        }

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (id == null || _context.Members == null)
            {
                return NotFound();
            }
            var member = await _context.Members.Include(m => m.User).FirstOrDefaultAsync(l => l.Id == id);
            if (member == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            var groups = await _context.Groups.ToListAsync();
            if (groups != null)
            {
                ViewData["GroupId"] = new SelectList(groups, "Id", "Name");
            }

            await LoadAsync(member);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string? id)
        {
            if (!ModelState.IsValid)
            {
                RedirectToPage();
            }
            var member = await _context.Members.Include(m => m.User).FirstOrDefaultAsync(l => l.Id == id);
            if (member == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var fName = member.User.FName;
            var lName = member.User.LName;
            var email = member.User.Email;
            var groupId = member.GroupId;
            if (Input.LName!= lName || Input.FName!=fName || Input.Email !=email)
            {
                if (Input.FName != fName)
                {
                    _context.Members.Find(member.Id).User.FName = Input.FName;

                }
                if (Input.LName != lName)
                {
                    _context.Members.Find(member.Id).User.LName = Input.LName;

                }
                if (Input.Email != email)
                {
                    _context.Members.Find(member.Id).User.Email = Input.Email;
                }

                if( Input.GroupId != groupId)
                {
                    _context.Members.Find(member.Id).GroupId = Input.GroupId;
                }

                await _context.SaveChangesAsync();

            }

            return RedirectToPage("./Index");
        }
    }
}
