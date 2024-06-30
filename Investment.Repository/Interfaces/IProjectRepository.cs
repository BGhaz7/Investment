using Investment.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Investment.Repository.Interfaces
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetAllProjectsAsync();
        Task<Project> GetProjectByIdAsync(Guid id);
        Task<Project> AddProjectAsync(Project project);
        Task UpdateProjectAsync(Project project);
    }
}