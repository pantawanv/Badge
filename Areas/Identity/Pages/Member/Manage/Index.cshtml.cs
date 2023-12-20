// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Badge.Areas.Identity.Data;
using Badge.Data;
using Badge.Interfaces;
using Badge.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Badge.Areas.Identity.Pages.AppMember.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IMemberService _memberService;

        public string ImgChange;
        public Member Member { get; set; }
        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context,
            IMemberService memberService)
        {
            _memberService = memberService;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public string StatusMessage { get; set; }
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public IFormFile? Image { get; set; }
            public string? ImageString { get; set; }
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Nuværende adgangskode")]
            public string OldPassword { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Ny adgangskode")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Bekræft adgangskode")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var member = await _memberService.GetMemberAsync(user.Id);
            if (member == null)
            {
                NotFound();
            }
            Member = member;
            var imageString = member.User.AppUImageData;
            Input = new InputModel
            {
                ImageString = imageString,
                Image = null
            };
        }

        public async Task<IActionResult> OnGetAsync(string? imgchange)
        {
            ImgChange = imgchange;
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            await LoadAsync(user);

            return Page();
        }

        //public async Task<IActionResult> OnPostAsync()
        //{
        //    var user = _userManager.GetUserAsync(User).Result;
        //    await _signInManager.RefreshSignInAsync(user);
        //    StatusMessage = "Your profile has been updated";
        //    return RedirectToPage();
        //}

        public async Task<IActionResult> OnPostUpdateImageAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (Input.Image != null)
            {

                user.ImageFile = Input.Image;
                byte[] bytes = null;
                using (Stream fs = user.ImageFile.OpenReadStream())
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        bytes = br.ReadBytes((Int32)fs.Length);
                    }
                }
                user.AppUImageData = Convert.ToBase64String(bytes, 0, bytes.Length);
            }
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteImageAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            user.ImageFile = null;
            user.AppUImageData = null;
            _context.Users.Update(user);
            _context.SaveChanges();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostChangePasswordAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your password has been changed.";

            return RedirectToPage();
        }
    }
}
