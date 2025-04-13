using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using testing.Data;
using testing.Data.Models;
using testing.Services.Interfaces;

namespace testing.Services
{
    public class TestCaseService : ITestCaseService
    {
        private readonly ApplicationDbContext _context;

        public TestCaseService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TestCase> CreateTestCaseAsync(TestCase testCase)
        {
            testCase.CreatedAt = DateTime.UtcNow;
            testCase.Status = TestCaseStatus.Draft;

            _context.TestCases.Add(testCase);
            await _context.SaveChangesAsync();

            return testCase;
        }

        public async Task<TestCase> GetTestCaseByIdAsync(int testCaseId)
        {
            return await _context.TestCases
                .Include(tc => tc.Project)
                .Include(tc => tc.AssignedTo)
                .Include(tc => tc.TestResults)
                    .ThenInclude(tr => tr.ExecutedBy)
                .FirstOrDefaultAsync(tc => tc.TestCaseId == testCaseId);
        }

        public async Task<List<TestCase>> GetTestCasesByProjectIdAsync(int projectId)
        {
            return await _context.TestCases
                .Include(tc => tc.AssignedTo)
                .Include(tc => tc.TestResults)
                .Where(tc => tc.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task<TestCase> UpdateTestCaseAsync(TestCase testCase)
        {
            var existingTestCase = await _context.TestCases.FindAsync(testCase.TestCaseId);
            if (existingTestCase == null)
                throw new KeyNotFoundException($"TestCase with ID {testCase.TestCaseId} not found.");

            existingTestCase.Title = testCase.Title;
            existingTestCase.Description = testCase.Description;
            existingTestCase.Steps = testCase.Steps;
            existingTestCase.ExpectedResult = testCase.ExpectedResult;
            existingTestCase.Priority = testCase.Priority;
            existingTestCase.Status = testCase.Status;
            existingTestCase.LastModified = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingTestCase;
        }

        public async Task<bool> DeleteTestCaseAsync(int testCaseId)
        {
            var testCase = await _context.TestCases.FindAsync(testCaseId);
            if (testCase == null)
                return false;

            _context.TestCases.Remove(testCase);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AssignTestCaseAsync(int testCaseId, int userId)
        {
            var testCase = await _context.TestCases.FindAsync(testCaseId);
            var user = await _context.Users.FindAsync(userId);

            if (testCase == null || user == null)
                return false;

            testCase.AssignedToUserId = userId;
            testCase.Status = TestCaseStatus.Ready;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<TestResult> ExecuteTestCaseAsync(int testCaseId, int userId, TestResult result)
        {
            var testCase = await _context.TestCases.FindAsync(testCaseId);
            if (testCase == null)
                throw new KeyNotFoundException($"TestCase with ID {testCaseId} not found.");

            result.TestCaseId = testCaseId;
            result.ExecutedByUserId = userId;
            result.ExecutedAt = DateTime.UtcNow;

            _context.TestResults.Add(result);
            await _context.SaveChangesAsync();

            // Update test case status based on result
            testCase.Status = result.Status switch
            {
                TestResultStatus.Passed => TestCaseStatus.Passed,
                TestResultStatus.Failed => TestCaseStatus.Failed,
                TestResultStatus.Blocked => TestCaseStatus.Blocked,
                _ => testCase.Status
            };

            await _context.SaveChangesAsync();

            return result;
        }

        public async Task<List<TestResult>> GetTestCaseHistoryAsync(int testCaseId)
        {
            return await _context.TestResults
                .Include(tr => tr.ExecutedBy)
                .Where(tr => tr.TestCaseId == testCaseId)
                .OrderByDescending(tr => tr.ExecutedAt)
                .ToListAsync();
        }
    }
} 