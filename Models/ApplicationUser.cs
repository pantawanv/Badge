using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Badge.Areas.Identity.Data
{
    public class ApplicationUser : IdentityUser
    {

        [PersonalData]
        [DisplayName("Fornavn")]
        [Required (ErrorMessage = "Fornavn skal udfyldes.")]
        public string? FName { get; set; }
        [PersonalData]
        [DisplayName("Efternavn")]
        [Required(ErrorMessage = "Efternavn skal udfyldes.")]
        public string? LName { get; set; }
        [PersonalData]
        [DisplayName("Profilbillede")]
        public string? AppUImageData { get; set; }

        [NotMapped]
        [DisplayName("Navn")]
        public string FullName { get { return FName + " " + LName; } }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
