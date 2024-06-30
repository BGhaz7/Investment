using Investment.Service.Interfaces;
using Investment.Models.Entities;
using Investment.Repository.Interfaces;

namespace Investment.Service.Services
{
    public class InvestmentService : IInvestmentService
    {
        private readonly IInvestmentRepository _investmentRepository;
        private readonly IProjectRepository _projectRepository;

        public InvestmentService(IInvestmentRepository investmentRepository, IProjectRepository projectRepository)
        {
            _investmentRepository = investmentRepository;
            _projectRepository = projectRepository;
        }

        public async Task<IEnumerable<InvestmentTransaction>> GetInvestmentByIdAsync(Guid projectId)
        {
            return await _investmentRepository.GetInvestmentsByProjectIdAsync(projectId);
        }

        public async Task AddInvestmentAsync(InvestmentTransaction investmentTransaction)
        { 
            await _investmentRepository.AddInvestmentAsync(investmentTransaction);
        }
        
        public async Task InvestAsync(int userId, Guid projectId, decimal amount)
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

            await _projectRepository.AddProjectAsync(project);
            await _investmentRepository.AddInvestmentAsync(investment);
        }
    }
}