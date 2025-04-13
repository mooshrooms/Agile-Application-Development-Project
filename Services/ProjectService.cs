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
    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext _context;

        public ProjectService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Project> CreateProjectAsync(Project project, int userId)
        {
            project.CreatedByUserId = userId;
            project.CreatedAt = DateTime.UtcNow;
            project.Status = ProjectStatus.Planning;

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            // Add creator as project manager
            await AssignUserToProjectAsync(project.ProjectId, userId, ProjectRole.Manager);

            return project;
        }

        public async Task<Project> GetProjectByIdAsync(int projectId)
        {
            return await _context.Projects
                .Include(p => p.CreatedBy)
                .Include(p => p.ProjectUsers)
                    .ThenInclude(pu => pu.User)
                .FirstOrDefaultAsync(p => p.ProjectId == projectId);
        }

        public async Task<List<Project>> GetAllProjectsAsync()
        {
            return await _context.Projects
                .Include(p => p.CreatedBy)
                .Include(p => p.ProjectUsers)
                    .ThenInclude(pu => pu.User)
                .ToListAsync();
        }

        public async Task<Project> UpdateProjectAsync(Project project)
        {
            var existingProject = await _context.Projects.FindAsync(project.ProjectId);
            if (existingProject == null)
                throw new KeyNotFoundException($"Project with ID {project.ProjectId} not found.");

            existingProject.Name = project.Name;
            existingProject.Description = project.Description;
            existingProject.Status = project.Status;
            existingProject.StartDate = project.StartDate;
            existingProject.EndDate = project.EndDate;

            await _context.SaveChangesAsync();
            return existingProject;
        }

        public async Task<bool> DeleteProjectAsync(int projectId)
        {
            var project = await _context.Projects.FindAsync(projectId);
            if (project == null)
                return false;

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AssignUserToProjectAsync(int projectId, int userId, ProjectRole role)
        {
            var project = await _context.Projects.FindAsync(projectId);
            var user = await _context.Users.FindAsync(userId);

            if (project == null || user == null)
                return false;

            var existingAssignment = await _context.ProjectUsers
                .FirstOrDefaultAsync(pu => pu.ProjectId == projectId && pu.UserId == userId);

            if (existingAssignment != null)
            {
                existingAssignment.Role = role;
            }
            else
            {
                _context.ProjectUsers.Add(new ProjectUser
                {
                    ProjectId = projectId,
                    UserId = userId,
                    Role = role
                });
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveUserFromProjectAsync(int projectId, int userId)
        {
            var projectUser = await _context.ProjectUsers
                .FirstOrDefaultAsync(pu => pu.ProjectId == projectId && pu.UserId == userId);

            if (projectUser == null)
                return false;

            _context.ProjectUsers.Remove(projectUser);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<User>> GetProjectUsersAsync(int projectId)
        {
            return await _context.ProjectUsers
                .Where(pu => pu.ProjectId == projectId)
                .Include(pu => pu.User)
                .Select(pu => pu.User)
                .ToListAsync();
        }
    }
} 