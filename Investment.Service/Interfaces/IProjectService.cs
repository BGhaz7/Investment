using Investment.Models.Entities;

namespace Investment.Service.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<Project>> GetProjectsAsync();
        Task<Project> GetProjectByIdAsync();
        Task AddProjectAsync(Project project);
    }
    
}