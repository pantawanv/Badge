using Badge.Areas.Identity.Data;
using Badge.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Badge.Services
{
    public class UserFactory : IUserFactory
    {
        public UserFactory() { }

        public ApplicationUser CreateUser()
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

        // Generates a random password for the new user
        public string CreateRandomPassword(int PasswordLength)
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
            // ¨Tilføjer en 2 cifret int, for at være sikker på at koden lever op til reglerne for passworded. 
            return (new string(chars))+randNum.NextInt64(2);
        }
    }

}

