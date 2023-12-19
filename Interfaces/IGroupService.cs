using Badge.Models;

namespace Badge.Interfaces
{
    public interface IGroupService
    {
        IQueryable<Group> GetGroups();

        Task CreateGroupAsync(Group group);

        Task<List<GroupType>> GetGroupTypesAsync();
    }
}
