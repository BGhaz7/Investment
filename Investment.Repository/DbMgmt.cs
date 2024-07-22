using Investment.Repository.DbContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Portfolio.Repository.DbContext
{
    public class DbMgmt
    {
        public static void MigrationInit(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                serviceScope.ServiceProvider.GetService<InvestmentContext>().Database.Migrate();
            }
        }
    }   
}