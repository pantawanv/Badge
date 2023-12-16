using Badge.Areas.Identity.Data;
using Badge.Data;
using Badge.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Text;
using System.Text.Encodings.Web;

namespace Badge.Pages.Admin.UserAdmin
{
    [Authorize(Policy = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<CreateModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly ApplicationDbContext _context;

        public CreateModel(UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            ILogger<CreateModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;

            _context = context;
        }

        [BindProperty]
        [DisplayName("Rolle")]
        public string RoleId { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            var roles = await _context.Roles.Where(r => r.NormalizedName != "ADMIN" && r.NormalizedName != "MEMBER").ToListAsync();
            List<SelectListItem> items = new List<SelectListItem>();

            foreach (var item in roles)
            {
                string roleid = item.Id;
                string rolename = TextResource.ResourceManager.GetString(item.Name);
                items.Add(new SelectListItem() { Text = rolename, Value = roleid });
            }
            ViewData["RoleId"] = items;
            return Page();
        }


        [BindProperty]
        public ApplicationUser User { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {


            returnUrl ??= Url.Content("~/Admin/Brugere/Details?id=");

            string phone = User.PhoneNumber;
            string email = User.Email;
            string fname = User.FName;
            string lname = User.LName;

            string role = _context.Roles.Find(RoleId).Name;

            User = CreateUser();

            string password = CreateRandomPassword(10);
            await _userStore.SetUserNameAsync(User, email, CancellationToken.None);
            await _emailStore.SetEmailAsync(User, email, CancellationToken.None);
            User.FName = fname;
            User.LName = lname;
            User.PhoneNumber = phone;
            var result = await _userManager.CreateAsync(User, password);

            if (!ModelState.IsValid)
            {
                RedirectToPage();
            }

            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");
                var userId = await _userManager.GetUserIdAsync(User);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(User);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl, password = password },
                    protocol: Request.Scheme); ;

                await _userManager.AddToRoleAsync(User, role);

                await _emailSender.SendEmailAsync(email, "Confirm your email",
                    $"Welcome to Badge!<br>Your account info is <br>username: {email}<br>password: {password}<br>Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                return LocalRedirect(returnUrl+userId);
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }


            // If we got this far, something failed, redisplay form
            return RedirectToPage();
        }

        public static string CreateRandomPassword(int PasswordLength)
        {
            string _allowedChars = "0123456789abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ";
            Random randNum = new Random();
            char[] chars = new char[PasswordLength];
            int allowedCharCount = _allowedChars.Length;
            string password;
            for (int i = 0; i < PasswordLength; i++)
            {
                chars[i] = _allowedChars[(int)((_allowedChars.Length) * randNum.NextDouble())];
            }
            return (new string(chars))+"23";
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
