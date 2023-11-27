using Microsoft.AspNetCore.Identity;

namespace Badge.Models
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string FName { get; set; }
        [PersonalData]
        public string LName { get; set; }
    }
}
