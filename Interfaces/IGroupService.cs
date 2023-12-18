using Badge.Models;

namespace Badge.Interfaces
{
    public interface IGroupService
    {
        IQueryable<Group> GetGroups();
    }
}
