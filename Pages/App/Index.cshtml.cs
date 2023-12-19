using Badge.Areas.Identity.Data;
using Badge.Data;
using Badge.Interfaces;
using Badge.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Badge.Pages.App
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAchievementService _achievementService;
        public IndexModel(UserManager<ApplicationUser> userManager, ISalesService salesService, IMemberService memberService, IAchievementService achievementService, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _achievementService = achievementService;

        }
        public List<Achievement> Achievements { get; set; }
        public async Task OnGetAsync()
        {
            var achievements = await _achievementService.GetAchievementsAsync();
            if (achievements == null)
            {
                NotFound();
            }
            Achievements = achievements;
        }

        public bool CheckTicketAchievement(int amount)
        {
            return _achievementService.CheckTicketAchievement(amount, _userManager.GetUserId(User));
        }

        public bool CheckChannelAchievement(string name)
        {
            return _achievementService.CheckChannelAchievement(name, _userManager.GetUserId(User));
        }

        public bool CheckGroupAchievement(int amount)
        {
            return _achievementService.CheckGroupAchievement(amount, _userManager.GetUserId(User));
        }
    }
}
