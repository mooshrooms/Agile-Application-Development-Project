using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace testing.Data.Models
{
    public class TestCase
    {
        [Key]
        public int TestCaseId { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Steps { get; set; }

        [Required]
        public string ExpectedResult { get; set; }

        [Required]
        public TestCasePriority Priority { get; set; }

        [Required]
        public TestCaseStatus Status { get; set; }

        public int? AssignedToUserId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? LastModified { get; set; }

        // Navigation properties
        public virtual Project Project { get; set; }
        public virtual User AssignedTo { get; set; }
        public virtual ICollection<TestResult> TestResults { get; set; }
    }

    public enum TestCasePriority
    {
        Low,
        Medium,
        High,
        Critical
    }

    public enum TestCaseStatus
    {
        Draft,
        Ready,
        InProgress,
        Passed,
        Failed,
        Blocked
    }
} 