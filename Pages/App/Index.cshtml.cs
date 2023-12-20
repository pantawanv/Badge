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
        private readonly IArticleService _articleService;
        public IndexModel(UserManager<ApplicationUser> userManager, IAchievementService achievementService, IArticleService articleService)
        {
            _userManager = userManager;
            _achievementService = achievementService;
            _articleService = articleService;

        }
        public List<Achievement> Achievements { get; set; }
        public Article Article { get; set; }
        public async Task OnGetAsync()
        {
            var achievements = await _achievementService.GetAchievementsAsync();
            if (achievements == null)
            {
                NotFound();
            }
            var article = await _articleService.GetArticleAsync(1);
            Achievements = achievements;
            Article = article;
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
