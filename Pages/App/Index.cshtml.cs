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
        private readonly ISalesService _salesService;
        private readonly IMemberService _memberService;
        private readonly IAchievementService _achievementService;
        public IndexModel(UserManager<ApplicationUser> userManager, ISalesService salesService, IMemberService memberService, IAchievementService achievementService, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _salesService = salesService;
            _memberService = memberService;
            _achievementService = achievementService;

        }
        public IList<Achievement> Achievements { get; set; }
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
            return amount <= GetSales();
        }

        public bool CheckChannelAchievement(string name)
        {
            switch (name)
            {
                case "Mobile Pay":
                    return _salesService.GetMembersSalesAsync(GetMember().Id).Result.Where(s=>s.Channel.Name == "Mobile Pay").Count() > 0;
                case "Kontant":
                    return _salesService.GetMembersSalesAsync(GetMember().Id).Result.Where(s=>s.Channel.Name == "Kontant").Count() > 0;
                default:
                    return false;
            }
        }

        public bool CheckGroupAchievement(int amount)
        {
            return amount <= _salesService.GetGroupSalesAsync(GetMember().Group.Id).Result.Count();
        }

        public Member GetMember()
        {
            return _memberService.GetMemberAsync(_userManager.GetUserId(User)).Result;
        }

        public int GetSales()
        {
            return _salesService.GetMembersSalesAsync(GetMember().Id).Result.Count();
        }
    }
}
