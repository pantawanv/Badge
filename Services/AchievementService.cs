using Badge.Data;
using Badge.Interfaces;
using Badge.Models;
using Microsoft.EntityFrameworkCore;

namespace Badge.Services
{
    public class AchievementService : IAchievementService
    {
        private readonly ApplicationDbContext _context;
        public AchievementService(ApplicationDbContext context)
        {
            _context=context;
        }

        public async Task<List<Achievement>> GetAchievementsAsync()
        {
            return await _context.Achievements.ToListAsync();
        }


    }
}
