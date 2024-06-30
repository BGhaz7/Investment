using Investment.Models.Entities;
using Investment.Repository.Interfaces;
using Investment.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Investment.Service.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IInvestmentRepository _investmentRepository;

        public ProjectService(IProjectRepository projectRepository, IInvestmentRepository investmentRepository)
        {
            _projectRepository = projectRepository;
            _investmentRepository = investmentRepository;
        }

        public async Task<Project> AddProjectAsync(Project project)
        {
            project.Id = Guid.NewGuid(); // New Guid
            return await _projectRepository.AddProjectAsync(project);
        }

        public async Task<Project> GetProjectByIdAsync(Guid id)
        {
            return await _projectRepository.GetProjectByIdAsync(id);
        }

        public async Task<IEnumerable<Project>> GetProjectsAsync()
        {
            return await _projectRepository.GetAllProjectsAsync();
        }

        public async Task InvestInProjectAsync(Guid projectId, int userId, decimal amount)
        {
            var project = await _projectRepository.GetProjectByIdAsync(projectId);
            if (project == null)
            {
                throw new KeyNotFoundException("Project not found.");
            }

            project.CurrentAmount += amount;

            var investment = new InvestmentTransaction
            {
                InvestmentId = Guid.NewGuid(),
                UserId = userId,
                ProjectId = projectId,
                Amount = amount,
                Date = DateTime.UtcNow
            };

            await _projectRepository.UpdateProjectAsync(project);
            await _investmentRepository.AddInvestmentAsync(investment);
        }
    }
}