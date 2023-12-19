using Badge.Models;

namespace Badge.Interfaces
{
    public interface IGroupService
    {
        IQueryable<Group> GetGroups();

        Task<Group> GetGroupAsync(int id);

        Task<List<Group>> GetGroupsByLeaderIdAsync(string id);

        Task CreateGroupAsync(Group group);

        Task DeleteGroupAsync(Group group);

        Task<List<GroupType>> GetGroupTypesAsync();
    }
}
