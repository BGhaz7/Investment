using Investment.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Investment.Repository.Interfaces
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetProjectsAsync();
        Task<Project> GetProjectByIdAsync(int id);
        Task AddProjectAsync(Project project);
    }
}