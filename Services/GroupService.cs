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

        // Henter alle grupper
         public IQueryable<Group> GetGroups()
        {
            return _context.Groups.AsQueryable();
        }

        // Henter gruppen hvis leder matcher id'et 

        public async Task<List<Group>> GetGroupsByLeaderIdAsync(string id)
        {
           return await _context.Groups.Include(g => g.Members).ThenInclude(g=>g.User).Include(g=>g.Members).ThenInclude(g=>g.Sales).Where(g => g.LeaderId == id).ToListAsync();
        }

        // Henter gruppe som matcher id
        public Task<Group> GetGroupAsync(int id)
        {
            return _context.Groups.SingleOrDefaultAsync(g => g.Id == id);
        }

        // Opretter en ny gruppe
        public async Task CreateGroupAsync(Group group)
        {
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();
        }

        // Sletter en gruppe

        public async Task DeleteGroupAsync(Group group)
        {
            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
        }

        // Henter alle gruppe typer
        public async Task<List<GroupType>> GetGroupTypesAsync()
        {
            return await _context.GroupTypes.ToListAsync();
        }
    }
}
