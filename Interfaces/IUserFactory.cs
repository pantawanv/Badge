using Badge.Areas.Identity.Data;

namespace Badge.Interfaces
{
    public interface IUserFactory
    {
        Task<ApplicationUser> CreateUserAsync();
        Task<string> CreateRandomPasswordAsync(int PasswordLength);
    }
}
