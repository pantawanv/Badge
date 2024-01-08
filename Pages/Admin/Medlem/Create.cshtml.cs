using Badge.Areas.Identity.Data;
using Badge.Data;
using Badge.Interfaces;
using Badge.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
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
        private readonly IUserFactory _userFactory;

        public CreateModel(UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            ILogger<CreateModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context,
            IUserFactory userFactory)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _userFactory = userFactory;

            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var groups = await _context.Groups.ToListAsync();
            ViewData["GroupId"] = new SelectList(groups, "Id", "Name");
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

            Member.User = await _userFactory.CreateUserAsync();

            string password = await _userFactory.CreateRandomPasswordAsync(8);
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

                var user= await _userManager.FindByEmailAsync(email);
                await _userManager.AddToRoleAsync(user, "Member");


                await _emailSender.SendEmailAsync(email, "Confirm your email",
                    $"Welcome to Badge!<br>Your account info is <br>username: {email}<br>password: {password}<br>Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                await _context.Members.AddAsync(Member);
                await _context.SaveChangesAsync();

                return RedirectToPage("Details", new { id = user.Id });
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }


            // If we got this far, something failed, redisplay form
            return Page();
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
