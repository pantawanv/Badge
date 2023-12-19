using Badge.Models;

namespace Badge.Interfaces
{
    public interface IAchievementService
    {
        Task<List<Achievement>> GetAchievementsAsync();

        bool CheckTicketAchievement(int amount, string userId);
        bool CheckChannelAchievement(string name, string userId);
        bool CheckGroupAchievement(int amount, string userId);
    }
}
