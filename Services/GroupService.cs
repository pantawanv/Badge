using Badge.Data;
using Badge.Interfaces;
using Badge.Models;
using Microsoft.EntityFrameworkCore;

namespace Badge.Services
{
    public class GroupService : IGroupService
    {
        private readonly ApplicationDbContext _context;
        public GroupService(ApplicationDbContext context)
        {
            _context = context;
        }

        //Get groups query
         public IQueryable<Group> GetGroups()
        {
            return _context.Groups.AsQueryable();
        }

        //Get groups by leaderid

        public async Task<List<Group>> GetGroupsByLeaderIdAsync(string id)
        {
           return await _context.Groups.Include(g => g.Members).ThenInclude(g=>g.User).Include(g=>g.Members).ThenInclude(g=>g.Sales).Where(g => g.LeaderId == id).ToListAsync();
        }

        //Find Group By Id
        public Task<Group> GetGroupAsync(int id)
        {
            return _context.Groups.SingleOrDefaultAsync(g => g.Id == id);
        }

        //Create group
        public async Task CreateGroupAsync(Group group)
        {
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();
        }

        //Delete group

        public async Task DeleteGroupAsync(Group group)
        {
            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
        }

        //Get group types
        public async Task<List<GroupType>> GetGroupTypesAsync()
        {
            return await _context.GroupTypes.ToListAsync();
        }




    }
}
