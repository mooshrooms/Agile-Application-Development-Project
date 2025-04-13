using System.Collections.Generic;
using System.Threading.Tasks;
using testing.Data.Models;

namespace testing.Services.Interfaces
{
    public interface IProjectService
    {
        Task<Project> CreateProjectAsync(Project project, int userId);
        Task<Project> GetProjectByIdAsync(int projectId);
        Task<List<Project>> GetAllProjectsAsync();
        Task<Project> UpdateProjectAsync(Project project);
        Task<bool> DeleteProjectAsync(int projectId);
        Task<bool> AssignUserToProjectAsync(int projectId, int userId, ProjectRole role);
        Task<bool> RemoveUserFromProjectAsync(int projectId, int userId);
        Task<List<User>> GetProjectUsersAsync(int projectId);
    }
} 