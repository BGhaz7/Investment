using Investment.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Investment.Repository.DbContext
{
    public class InvestmentContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public InvestmentContext(DbContextOptions<InvestmentContext> options) : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<InvestmentTransaction> Investments { get; set; }
    }
}