using Badge.Areas.Identity.Data;
using Badge.Data;
using Badge.Interfaces;
using Badge.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Badge.Services
{
    public class AchievementService : IAchievementService
    {
        private readonly ApplicationDbContext _context;
        private readonly ISalesService _salesService;
        private readonly IMemberService _memberService;
        public AchievementService(ApplicationDbContext context, ISalesService salesService, IMemberService memberService)
        {
            _context=context;
            _memberService=memberService;
            _salesService=salesService;
        }

        // Tjekker om medlemmet har lavet et salg med salgskanalen som matcher navnet på achievementen
        public bool CheckChannelAchievement(string name, string userId)
        {
            switch (name)
            {
                case "Mobile Pay":
                    return _salesService.GetMembersSalesAsync(userId).Result.Where(s => s.Channel.Name == "Mobile Pay").Count() > 0;
                case "Kontant":
                    return _salesService.GetMembersSalesAsync(userId).Result.Where(s => s.Channel.Name == "Kontant").Count() > 0;
                default:
                    return false;
            }
        }
        // Tjekker om medlemmerne i gruppen tilsammen har lavet <= salg end det angivne antal 
        public bool CheckGroupAchievement(int amount, string userId)
        {
            return amount <= _salesService.GetGroupSalesAsync(_memberService.GetMemberAsync(userId).Result.Group.Id).Result.Count();
        }


        // Tjekker om medlemmet har lavet <= salg en det angivne antal
        public bool CheckTicketAchievement(int amount, string userId)
        {
            return amount <=  _salesService.GetMembersSalesAsync(userId).Result.Count();
        }

        // Henter alle Achievements
        public async Task<List<Achievement>> GetAchievementsAsync()
        {
            return await _context.Achievements.Include(a => a.AchievementType).ToListAsync();
        }

    }
}
