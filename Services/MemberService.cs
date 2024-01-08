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

        // Henter medlem som matcher id
        public async Task<Member> GetMemberAsync(string id)
        {
            return await _context.Members.Include(m => m.User).Include(m => m.Group).ThenInclude(m => m.GroupType).Include(m => m.Group).ThenInclude(m => m.Leader).FirstOrDefaultAsync(m => m.Id == id);
        }

        // Henter alle medlemmer
        public async Task<List<Member>> GetAllMembersAsync()
        {
            return await _context.Members.Include(m=>m.Sales).Include(m => m.User).Include(m => m.Group).ThenInclude(m => m.GroupType).Include(m => m.Group).ThenInclude(m => m.Leader).ToListAsync();
        }

        // Henter alle medlemmer som er i gruppe med det angivne id
        public async Task<List<Member>> GetAllMembersOfGroupAsync(int id)
        {
            return await _context.Members.Include(m => m.User).Include(m => m.Group).ThenInclude(m => m.GroupType).Include(m => m.Group).ThenInclude(m => m.Leader).Where(m => m.GroupId == id).ToListAsync();
        }

        // Henter forældrene for et medlem med det angivne id
        public async Task<List<Parent>> GetParentsOfMemberAsync(string id)
        {
            return await _context.Parents.Where(p => p.Members.Where(m => m.MemberId == id).Any()).ToListAsync();
        }

        // Henter medlemmet for en forældre med det angivne id
        public async Task<List<Member>> GetMembersOfParentsAsync(int id)
        {
            var members = _context.MemberParents.Include(m => m.Member).ThenInclude(m => m.User).Where(m => m.ParentId == id);
            return await members.Select(m => m.Member).ToListAsync();
        }

        // Henter en forældre med det angivne id
        public Parent GetParent(int id)
        {
            return _context.Parents.Include(p => p.Members).ThenInclude(p => p.Member).ThenInclude(p=>p.User).Include(p => p.Members).ThenInclude(p => p.Member).ThenInclude(p => p.Group).Where(p => p.Id == id).FirstOrDefault();
        }

        // Henter alle forældre
        public IQueryable<Parent> GetParentsQuery()
        {
            return _context.Parents.Include(p => p.Members).ThenInclude(p => p.Member).ThenInclude(p => p.User).Include(p => p.Members).ThenInclude(p => p.Member).ThenInclude(p => p.Group).AsQueryable();
        }

        // Henter alle medlemmer
        public IQueryable<Member> GetMembers()
        {
            return _context.Members.Include(m=>m.User).Include(m => m.Sales).Include(m => m.Group).ThenInclude(m => m.GroupType).AsQueryable();
        }

        // Opdaterer et medlem
        public async Task UpdateMemberAsync(Member member)
        {
            _context.Update(member);
            await _context.SaveChangesAsync();
        }

        // Sleter et medlem 
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
