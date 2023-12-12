using Badge.Areas.Identity.Data;
using Badge.Data;
using Badge.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;

namespace Badge.Pages.Admin.MemberAdmin
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<CreateModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;

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

        public IActionResult OnGet()
        {
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Models.Member Member { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/Admin/Medlem/Details?id=");

            string email = Member.User.Email;
            string fname = Member.User.FName;
            string lname = Member.User.LName;

            string role = "Member";

            Member.User = CreateUser();

            string password = CreateRandomPassword(10);
            await _userStore.SetUserNameAsync(Member.User, email, CancellationToken.None);
            await _emailStore.SetEmailAsync(Member.User, email, CancellationToken.None);
            Member.User.FName = fname;
            Member.User.LName = lname;
            var result = await _userManager.CreateAsync(Member.User, password);

            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");
                var userId = await _userManager.GetUserIdAsync(Member.User);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(Member.User);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl, password = password },
                    protocol: Request.Scheme); ;

                var roleresult = await _userManager.AddToRoleAsync(Member.User, role);

                await _emailSender.SendEmailAsync(email, "Confirm your email",
                    $"Welcome to Badge!<br>Your account info is <br>username: {email}<br>password: {password}<br>Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                await _context.Members.AddAsync(Member);
                await _context.SaveChangesAsync();

                return LocalRedirect(returnUrl+userId);
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }


            // If we got this far, something failed, redisplay form
            return Page();
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
