using Badge.Areas.Identity.Data;
using Badge.Data;
using Badge.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
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
        private readonly IUserFactory _userFactory;

        public CreateModel(UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            IUserFactory userFactory,
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
            _userFactory = userFactory;
        }

        [BindProperty]
        [DisplayName("Rolle")]
        public string RoleId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            List<SelectListItem> roles = new List<SelectListItem>();

            roles.Add(new SelectListItem() { Text = "Leader", Value = (await _roleManager.FindByNameAsync("Leader")).Id });
            roles.Add(new SelectListItem() { Text = "Manager", Value = (await _roleManager.FindByNameAsync("Manager")).Id });
            
            ViewData["RoleId"] = roles;
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

            
            string role = (await _roleManager.FindByIdAsync(RoleId)).Name;

            // Opretter bruger 
            User = await _userFactory.CreateUserAsync();

            // Genererer password 
            string password = await _userFactory.CreateRandomPasswordAsync(10);

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
                    values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl},
                    protocol: Request.Scheme); ;

                await _userManager.AddToRoleAsync(User, role);

                // Sender email til den oprettede bruger 
                await _emailSender.SendEmailAsync(email, "Bekræft din email",
                    $"Velkommen til Badge!<br>Din bruger info er <br>brugernavn: {email}<br>password: {password}<br>Du kan bekræfte din konto ved at <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>klikke her!</a>.");
                
                // Redirecter til detalje siden 
                return RedirectToPage("Details", new { id = userId });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            // If we got this far, something failed, redisplay form
            return RedirectToPage();
        }


        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
