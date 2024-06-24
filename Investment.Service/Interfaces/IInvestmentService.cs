using Investment.Models.Entities;

namespace Investment.Service.Interfaces
{
    public interface IInvestmentService
    {
        Task<IEnumerable<InvestmentTransaction>> GetInvestmentByIdAsync(int projectId);
        Task AddInvestmentAsync(InvestmentTransaction investmentTransaction);
        Task InvestAsync(int userId, int projectId, decimal amount);
    }
}