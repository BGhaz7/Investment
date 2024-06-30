using Investment.Models.Entities;

namespace Investment.Service.Interfaces
{
    public interface IInvestmentService
    {
        Task<IEnumerable<InvestmentTransaction>> GetInvestmentByIdAsync(Guid projectId);
        Task AddInvestmentAsync(InvestmentTransaction investmentTransaction);
        Task InvestAsync(int userId, Guid projectId, decimal amount);
    }
}