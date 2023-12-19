using Badge.Areas.Identity.Data;
using Badge.Data;
using Badge.Interfaces;
using Badge.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
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
        private readonly IMemberService _memberService;

        public EditModel(UserManager<ApplicationUser> userManager,
            ApplicationDbContext context, IMemberService memberService)
        {
            _userManager = userManager;
            _context = context;
            _memberService = memberService;
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
            if (id == null || _memberService.GetMembers == null)
            {
                return NotFound();
            }
            var member = await _memberService.GetMemberAsync(id);
            if (!User.IsInRole("Manager") && _userManager.GetUserId(User) != member.Group.LeaderId)
            {
                return Forbid();
            }
            
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
            var member = await _memberService.GetMemberAsync(id);
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
                  (await _memberService.GetMemberAsync(id)).User.FName = Input.FName;

                }
                if (Input.LName != lName)
                {
                    (await _memberService.GetMemberAsync(id)).User.LName = Input.LName;

                }
                if (Input.Email != email)
                {
                    (await _memberService.GetMemberAsync(id)).User.Email = Input.Email;
                }

                if (Input.GroupId != groupId)
                {
                    (await _memberService.GetMemberAsync(id)).GroupId = Input.GroupId;
                }

                await _memberService.UpdateMemberAsync(member);

            }

            return RedirectToPage("./Index");
        }
    }
}
