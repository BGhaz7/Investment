using Microsoft.EntityFrameworkCore;
using Investment.Models.Entities;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Investment.Repository.DbContext
{
    public class InvestmentContextFactory : IDesignTimeDbContextFactory<InvestmentContext>
    {
        public InvestmentContext CreateDbContext(string[] args)
        {
            var basePath = Directory.GetCurrentDirectory();
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile(Path.Combine(basePath, "../Investment.API/appsettings.json"))
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<InvestmentContext>();
            var connectionString = configuration.GetConnectionString("PostGresConnectionString");
            optionsBuilder.UseNpgsql(connectionString);

            return new InvestmentContext(optionsBuilder.Options);
        }
    }
}