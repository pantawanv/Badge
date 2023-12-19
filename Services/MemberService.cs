using Badge.Areas.Identity.Data;
using Badge.Data;
using Badge.Interfaces;
using Badge.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Badge.Services
{
    public class MemberService : IMemberService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public MemberService(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _context=context;
            _userManager=userManager;
        }

        public async Task<Member> GetMemberAsync(string id)
        {
            return await _context.Members.Include(m => m.User).Include(m => m.Group).ThenInclude(m => m.GroupType).Include(m => m.Group).ThenInclude(m => m.Leader).FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<List<Member>> GetAllMembersAsync()
        {
            return await _context.Members.Include(m=>m.Sales).Include(m => m.User).Include(m => m.Group).ThenInclude(m => m.GroupType).Include(m => m.Group).ThenInclude(m => m.Leader).ToListAsync();
        }

        public async Task<List<Member>> GetAllMembersOfGroupAsync(int id)
        {
            return await _context.Members.Include(m => m.User).Include(m => m.Group).ThenInclude(m => m.GroupType).Include(m => m.Group).ThenInclude(m => m.Leader).Where(m => m.GroupId == id).ToListAsync();
        }

        public async Task<List<Parent>> GetParentsOfMemberAsync(string id)
        {
            return await _context.Parents.Where(p => p.Members.Where(m => m.MemberId == id).Any()).ToListAsync();
        }

        public async Task<List<Member>> GetMembersOfParentsAsync(int id)
        {
            var members = _context.MemberParents.Include(m => m.Member).ThenInclude(m => m.User).Where(m => m.ParentId == id);
            return await members.Select(m => m.Member).ToListAsync();
        }

        public Parent GetParent(int id)
        {
            return _context.Parents.Include(p => p.Members).ThenInclude(p => p.Member).ThenInclude(p=>p.User).Include(p => p.Members).ThenInclude(p => p.Member).ThenInclude(p => p.Group).Where(p => p.Id == id).FirstOrDefault();
        }

        public IQueryable<Parent> GetParentsQuery()
        {
            return _context.Parents.Include(p => p.Members).ThenInclude(p => p.Member).ThenInclude(p => p.User).Include(p => p.Members).ThenInclude(p => p.Member).ThenInclude(p => p.Group).AsQueryable();
        }
        public IQueryable<Member> GetMembers()
        {
            return _context.Members.Include(m=>m.User).Include(m => m.Sales).Include(m => m.Group).ThenInclude(m => m.GroupType).AsQueryable();
        }
        public async Task UpdateMemberAsync(Member member)
        {
            _context.Update(member);
            _context.SaveChanges();
        }

        public async Task DeleteMemberAsync(Member member)
        {
            if (member != null)
            {
                var user = await _userManager.FindByIdAsync(member.Id);
                if (user != null)
                {
                    await _userManager.DeleteAsync(member.User);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
