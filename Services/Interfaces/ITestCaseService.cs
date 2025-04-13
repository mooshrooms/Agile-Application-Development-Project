using System.Collections.Generic;
using System.Threading.Tasks;
using testing.Data.Models;

namespace testing.Services.Interfaces
{
    public interface ITestCaseService
    {
        Task<TestCase> CreateTestCaseAsync(TestCase testCase);
        Task<TestCase> GetTestCaseByIdAsync(int testCaseId);
        Task<List<TestCase>> GetTestCasesByProjectIdAsync(int projectId);
        Task<TestCase> UpdateTestCaseAsync(TestCase testCase);
        Task<bool> DeleteTestCaseAsync(int testCaseId);
        Task<bool> AssignTestCaseAsync(int testCaseId, int userId);
        Task<TestResult> ExecuteTestCaseAsync(int testCaseId, int userId, TestResult result);
        Task<List<TestResult>> GetTestCaseHistoryAsync(int testCaseId);
    }
} 