using Badge.Areas.Identity.Data;
using Badge.Interfaces;
namespace Badge.Services
{
    public class UserFactory : IUserFactory
    {
        // Opretter en instans af en bruger 
        public async Task<ApplicationUser> CreateUserAsync()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'.");
            }
        }

        // Genererer et random password for brugeren 
        public async Task<string> CreateRandomPasswordAsync(int PasswordLength)
        {
            string _allowedChars = "0123456789abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ";
            Random randNum = new Random();
            char[] chars = new char[PasswordLength];
            string password;
            for (int i = 0; i < PasswordLength; i++)
            {
                chars[i] = _allowedChars[(int)((_allowedChars.Length) * randNum.NextDouble())];
            }
            // Returnerer koden
            // Tilføjer en 2 cifret int, for at være sikker på at koden lever op til reglerne for passworded. 
            return (new string(chars))+randNum.NextInt64(2);
        }
    }

}

