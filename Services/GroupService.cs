using Badge.Data;
using Badge.Interfaces;
using Badge.Models;

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

    }
}
