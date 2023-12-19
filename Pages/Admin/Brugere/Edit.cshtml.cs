using Badge.Areas.Identity.Data;
using Badge.Data;
using Badge.Models;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Badge.Pages.Admin.UserAdmin
{
    public class EditModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public EditModel(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }


        public string Username { get; set; }
        [TempData]
        public string StatusMessage { get; set; }
        [BindProperty]
        public InputModel Input { get; set; }

        public List<IdentityRole> Roles { get; set; }

        public class InputModel
        {
            public string Id { get; set; }
            public string FName { get; set; }
            public string LName { get; set; }
            public string Email { get; set; }
            [Phone]
            [Display(Name = "Telefon nummer")]
            public string PhoneNumber { get; set; }
            public string RoleId { get; set; }
        }
        

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var fName = user.FName;
            var lName = user.LName;
            var email = user.Email;
            var id = user.Id;

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FName = fName,
                LName = lName,
                Email = email,
                Id = id
            };
        }

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (id == null || _userManager.Users == null)
            {
                return NotFound();
            }
            var userroles = from r in _context.Roles where (from ur in _context.UserRoles where ur.UserId == id && ur.RoleId == r.Id select ur).Any() select r;
            Roles = await userroles.ToListAsync();
            var leader = await _userManager.FindByIdAsync(id);
            if (leader == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            List<SelectListItem> roles = new List<SelectListItem>();
            roles.Add(new SelectListItem() { Text = "Leader", Value = _roleManager.FindByNameAsync("Leader").Result.Id });
            roles.Add(new SelectListItem() { Text = "Manager", Value = _roleManager.FindByNameAsync("Manager").Result.Id });

            ViewData["RoleId"] = roles;
            await LoadAsync(leader);
            return Page();
        }

        public async Task<IActionResult> OnPostEditAsync(string id)
        {
            var leader = await _userManager.FindByIdAsync(id);
            if (leader == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            
            var fName = leader.FName;
            var lName = leader.LName;
            var phoneNumber = leader.PhoneNumber;
            var email = leader.Email;
            if (Input.LName!= lName || Input.FName!=fName || Input.PhoneNumber !=phoneNumber || Input.Email !=email)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (Input.FName != fName)
                {
                   user.FName = fName;

                }
                if (Input.LName != lName)
                {
                    user.LName = lName;

                }

                if (Input.PhoneNumber != phoneNumber)
                {
                    user.PhoneNumber = Input.PhoneNumber;
                }
                if (Input.Email != email)
                {
                    user.Email = Input.Email;
                }

                await _userManager.UpdateAsync(user);

            }

            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostAddRoleAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null) { return NotFound(); }
            var role = await _roleManager.FindByIdAsync(Input.RoleId);
            await _userManager.AddToRoleAsync(user, role.Name);

            return LocalRedirect("");
        }

        public async Task<IActionResult> OnPostDeleteRoleAsync(string id, string roleid)
        {
            var user = await _userManager.FindByIdAsync(id);
            if(user == null) { return NotFound(); }
            var role = await _roleManager.FindByIdAsync(roleid);
            await _userManager.RemoveFromRoleAsync(user, role.Name);
            return RedirectToPage("Details", new { id = Input.Id });
        }
    }
}
