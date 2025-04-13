using System;
using System.ComponentModel.DataAnnotations;

namespace testing.Data.Models
{
    public class TestResult
    {
        [Key]
        public int TestResultId { get; set; }

        [Required]
        public int TestCaseId { get; set; }

        [Required]
        public int ExecutedByUserId { get; set; }

        [Required]
        public TestResultStatus Status { get; set; }

        public string ActualResult { get; set; }
        public string Notes { get; set; }

        [Required]
        public DateTime ExecutedAt { get; set; }

        public TimeSpan? ExecutionTime { get; set; }

        // Navigation properties
        public virtual TestCase TestCase { get; set; }
        public virtual User ExecutedBy { get; set; }
    }

    public enum TestResultStatus
    {
        Passed,
        Failed,
        Blocked,
        NotExecuted
    }
} 