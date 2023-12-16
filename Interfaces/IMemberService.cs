using Badge.Models;

namespace Badge.Interfaces
{
    public interface IMemberService
    {
        Task<Member> GetMemberAsync(string id);
    }
}
