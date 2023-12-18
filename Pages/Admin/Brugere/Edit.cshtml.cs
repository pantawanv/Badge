using Badge.Areas.Identity.Data;
using Badge.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Badge.Pages.Admin.UserAdmin
{
    public class EditModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public EditModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        public string Username { get; set; }
        [TempData]
        public string StatusMessage { get; set; }
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string FName { get; set; }
            public string LName { get; set; }
            public string Email { get; set; }
            [Phone]
            [Display(Name = "Telefon nummer")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var fName = user.FName;
            var lName = user.LName;
            var email = user.Email;

            Username = userName;

            Input = new InputModel
            {

                PhoneNumber = phoneNumber,
                FName = fName,
                LName = lName,
                Email = email,
            };
        }

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (id == null || _userManager.Users == null)
            {
                return NotFound();
            }

            var leader = await _userManager.FindByIdAsync(id);
            if (leader == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(leader);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string? id)
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

            return RedirectToPage("./Index");
        }
    }
}
