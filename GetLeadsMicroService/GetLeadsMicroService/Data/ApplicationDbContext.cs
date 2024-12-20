using GetLeadsMicroService.Models;
using Microsoft.EntityFrameworkCore;

namespace GetLeadsMicroService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Lead> Leads { get; set; }
    }

}
