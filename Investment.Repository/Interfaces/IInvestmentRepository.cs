using Investment.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Investment.Repository.Interfaces
{
    public interface IInvestmentRepository
    {
        Task<IEnumerable<InvestmentTransaction>> GetInvestmentsByProjectIdAsync(Guid projectId);
        Task AddInvestmentAsync(InvestmentTransaction investment);
    }
}
