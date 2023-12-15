using Badge.Areas.Identity.Data;
using Badge.Data;
using Badge.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Badge.Pages.App
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        public IndexModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
            return amount <= _context.Sales.Where(s => s.SellerId == GetMember().Id).Count();
        }

        public bool CheckChannelAchievement(int amount, string name)
        {
            switch (name)
            {
                case "Mobile Pay":
                    return _context.Sales.Where(s => s.SellerId == GetMember().Id && s.Channel.Name == "Mobile Pay").Count() > 0;
                    break;
                case "Kontant":
                    return _context.Sales.Where(s => s.SellerId == GetMember().Id && s.Channel.Name == "Kontakt").Count() > 0;
                    break;
                default:
                    return false;
                    break;
            }
        }

        public bool CheckGroupAchievement(int amount)
        {
            return amount <= _context.Sales.Where(s => s.Seller.GroupId == GetMember().GroupId).Count();
        }

        public Member GetMember()
        {
            return _context.Members.FirstOrDefault(m => m.Id == _userManager.GetUserId(User));
        }

        public int GetSales()
        {
            return _context.Sales.Where(s => s.SellerId == GetMember().Id).Count();
        }
    }
}
