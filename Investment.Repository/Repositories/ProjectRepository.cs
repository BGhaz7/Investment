using Investment.Models.Entities;
using Investment.Repository.DbContext;
using Investment.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Investment.Repository.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly InvestmentContext _context;

        public ProjectRepository(InvestmentContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Project>> GetProjectsAsync()
        {
            return await _context.Projects.ToListAsync();
        }

        public async Task<Project> GetProjectByIdAsync(int id)
        {
            return await _context.Projects.FindAsync(id);
        }

        public async Task AddProjectAsync(Project project)
        {
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
        }
    }
}