using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace testing.Data.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public int CreatedByUserId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [Required]
        public ProjectStatus Status { get; set; }

        // Navigation properties
        public virtual User CreatedBy { get; set; }
        public virtual ICollection<TestCase> TestCases { get; set; }
        public virtual ICollection<ProjectUser> ProjectUsers { get; set; }
    }

    public enum ProjectStatus
    {
        Planning,
        InProgress,
        Testing,
        Completed,
        OnHold,
        Cancelled
    }
} 