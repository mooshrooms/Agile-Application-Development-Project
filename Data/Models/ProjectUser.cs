using System.ComponentModel.DataAnnotations;

namespace testing.Data.Models
{
    public class ProjectUser
    {
        [Required]
        public int ProjectId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public ProjectRole Role { get; set; }

        // Navigation properties
        public virtual Project Project { get; set; }
        public virtual User User { get; set; }
    }

    public enum ProjectRole
    {
        Manager,
        Tester,
        Developer,
        Stakeholder
    }
} 