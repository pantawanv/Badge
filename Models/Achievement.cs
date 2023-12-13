using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Badge.Models
{
    public class Achievement
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Navn")]
        public string Name { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
        public int AchievementTypeId { get; set; }
        [ForeignKey(nameof(AchievementTypeId))]
        public AchievementType AchievementType { get; set; }

    }
}
