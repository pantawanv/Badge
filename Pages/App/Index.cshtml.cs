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
        private readonly ApplicationDbContext _context;
        public IndexModel(UserManager<ApplicationUser> userManager, ISalesService salesService, IMemberService memberService, SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _salesService = salesService;
            _memberService = memberService;
            _context = context;

        }
        public IList<Achievement> Achievements { get; set; }
        public async Task OnGetAsync()
        {
            var achivements = await _context.Achievements.Include(a => a.AchievementType).ToListAsync();
            if (achivements == null)
            {

            }
            Achievements = achivements;
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
