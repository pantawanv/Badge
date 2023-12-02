using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

namespace Badge.Areas.Identity.Data
{
    public class ApplicationUser : IdentityUser
    {

        [PersonalData]
        [DisplayName("Fornavn")]
        public string? FName { get; set; }
        [PersonalData]
        [DisplayName("Fornavn")]
        public string? LName { get; set; }
    }
}
