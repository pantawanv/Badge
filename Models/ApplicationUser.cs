using Microsoft.AspNetCore.Identity;

namespace Badge.Areas.Identity.Data
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string? FName { get; set; }
        [PersonalData]
        public string? LName { get; set; }
    }
}
