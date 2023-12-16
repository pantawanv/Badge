using Badge.Data;
using Badge.Interfaces;
using Badge.Models;
using Microsoft.EntityFrameworkCore;

namespace Badge.Services
{
    public class MemberService : IMemberService
    {
        private readonly ApplicationDbContext _context;
        public MemberService(ApplicationDbContext context)
        {
            _context=context;
        }

        public async Task<Member> GetMemberAsync(string id)
        {
            return await _context.Members.Include(m => m.User).Include(m => m.Group).ThenInclude(m => m.GroupType).Include(m => m.Group).ThenInclude(m => m.Leader).FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<List<Member>> GetAllMembersAsync()
        {
            return await _context.Members.Include(m => m.User).Include(m => m.Group).ThenInclude(m => m.GroupType).Include(m => m.Group).ThenInclude(m => m.Leader).ToListAsync();
        }

        public async Task<List<Member>> GetAllMembersOfGroupAsync(int id)
        {
            return await _context.Members.Include(m => m.User).Include(m => m.Group).ThenInclude(m => m.GroupType).Include(m => m.Group).ThenInclude(m => m.Leader).Where(m => m.GroupId == id).ToListAsync();
        }
    }
}
