using Badge.Models;

namespace Badge.Interfaces
{
    public interface IAchievementService
    {
        Task<List<Achievement>> GetAchievementsAsync();
    }
}
