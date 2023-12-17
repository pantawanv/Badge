using Badge.Models;

namespace Badge.Interfaces
{
    public interface IMemberService
    {
        Task<Member> GetMemberAsync(string id);
        Task<List<Member>> GetAllMembersAsync();
        Task<List<Member>> GetAllMembersOfGroupAsync(int id);
        Task<List<Parent>> GetParentsOfMemberAsync(string id);
        Task<List<Member>> GetMembersOfParentsAsync(int id);
    }
}
