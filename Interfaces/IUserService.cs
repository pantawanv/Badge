using Badge.Areas.Identity.Data;

namespace Badge.Interfaces
{
    public interface IUserService
    {
        IQueryable<ApplicationUser> GetUsers();
    }
}
