using Badge.Data;

namespace Badge.Services
{
    public class AchievementService
    {
        public readonly ApplicationDbContext _context;
        public AchievementService(ApplicationDbContext context) 
        {
            _context = context;
        }
    }
}
