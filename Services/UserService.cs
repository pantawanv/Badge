using Badge.Areas.Identity.Data;
using Badge.Data;
using Badge.Interfaces;

namespace Badge.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        public UserService(ApplicationDbContext context) 
        { 
            _context = context;
        }

        public IQueryable<ApplicationUser> GetUsers()
        {
            return _context.Users.AsQueryable();
        }

    }
}
