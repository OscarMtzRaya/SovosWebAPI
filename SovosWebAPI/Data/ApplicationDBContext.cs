using Microsoft.EntityFrameworkCore;
using SovosWebProject.Models;

namespace SovosWebProject.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }



        protected ApplicationDBContext()
        {
        }
    }
}
