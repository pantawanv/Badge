// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Badge.Areas.Identity.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Badge.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<ApplicationUser> signInManager, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }


        [BindProperty]
        public InputModel Input { get; set; }


        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {

            [Required]
            [EmailAddress]
            [Display(Name ="Mailadresse")]
            public string Email { get; set; }


            [Required]
            [DataType(DataType.Password)]
            [Display(Name ="Adgangskode")]
            public string Password { get; set; }


            [Display(Name = "Husk mig?")]
            public bool RememberMe { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string? usertype, string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            if (_signInManager.IsSignedIn(User))
            {
                return LocalRedirect(LogInUrl());
            }

            returnUrl ??= Url.Content("~/");

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string? usertype, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    if (usertype != null)
                    {
                        if (usertype == "Leader")
                        {
                            if (User.IsInRole("Leader") ||  User.IsInRole("Manager") ||  User.IsInRole("Admin"))
                            {
                                _logger.LogInformation("User logged in.");

                                return LocalRedirect("~/Admin/Index");
                            }
                            else
                            {
                                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
                                ModelState.AddModelError("NotLeader", string.Empty);
                                return Page();
                            }
                        }
                        else if (usertype == "Member")
                        {
                            if (User.IsInRole("Member"))
                            {
                                _logger.LogInformation("User logged in.");


                                return LocalRedirect("~/App/Index");
                            }
                            else
                            {
                                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
                                ModelState.AddModelError("NotMember", string.Empty);
                                return Page();
                            }
                        }

                    }
                    return LocalRedirect(LogInUrl());

                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        public string LogInUrl()
        {
            if (User.IsInRole("Member"))
            {
                return ("~/App/Index");
            }
            if (User.IsInRole("Admin") || User.IsInRole("Leader") || User.IsInRole("Manager"))
            {
                return ("~/Admin/Index");
            }
            return "";
        }
    }
}
