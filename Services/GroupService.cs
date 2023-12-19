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

         public IQueryable<Group> GetGroups()
        {
            return _context.Groups.AsQueryable();
        }

        public async Task CreateGroupAsync(Group group)
        {
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();
        }

        public async Task<List<GroupType>> GetGroupTypesAsync()
        {
            return await _context.GroupTypes.ToListAsync();
        }

    }
}
