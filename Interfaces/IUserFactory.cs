using Badge.Areas.Identity.Data;

namespace Badge.Interfaces
{
    public interface IUserFactory
    {
        ApplicationUser CreateUser();
        string CreateRandomPassword(int PasswordLength);
    }
}
