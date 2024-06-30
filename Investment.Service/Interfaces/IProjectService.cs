using Investment.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Investment.Service.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<Project>> GetProjectsAsync();
        Task<Project> GetProjectByIdAsync(Guid id);
        Task<Project> AddProjectAsync(Project project);
        Task InvestInProjectAsync(Guid projectId, int userId, decimal amount);
    }
}