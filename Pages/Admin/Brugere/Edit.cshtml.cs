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
        private readonly ApplicationDbContext _context;

        public EditModel(UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }


        public string Username { get; set; }
        [TempData]
        public string StatusMessage { get; set; }
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            public string FName { get; set; }
            public string LName { get; set; }
            public string Email { get; set; }

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
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var leader = await _context.Users.FirstOrDefaultAsync(l => l.Id == id);
            if (leader == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(leader);
            return Page();
        }


        public async Task<IActionResult> OnPostAsync(string? id)
        {
            var leader = await _context.Users.FirstOrDefaultAsync(l => l.Id == id);
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
                if (Input.FName != fName)
                {
                    _context.Users.Find(leader.Id).FName = Input.FName;

                }
                if (Input.LName != lName)
                {
                    _context.Users.Find(leader.Id).LName = Input.LName;

                }

                if (Input.PhoneNumber != phoneNumber)
                {
                    _context.Users.Find(leader.Id).PhoneNumber = Input.PhoneNumber;
                }
                if (Input.Email != email)
                {
                    _context.Users.Find(leader.Id).Email = Input.Email;
                }

                await _context.SaveChangesAsync();

            }

            return RedirectToPage("./Index");
        }
    }
}
