using Investment.Models.Entities;
using Investment.Repository.DbContext;
using Investment.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Investment.Repository.Repositories
{
    public class InvestmentRepository : IInvestmentRepository
    {
        private readonly InvestmentContext _context;

        public InvestmentRepository(InvestmentContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<InvestmentTransaction>> GetInvestmentsByProjectIdAsync(int projectId)
        {
            return await _context.Investments
                .Where(i => i.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task AddInvestmentAsync(InvestmentTransaction investment)
        {
            await _context.Investments.AddAsync(investment);
            await _context.SaveChangesAsync();
        }
    }
}