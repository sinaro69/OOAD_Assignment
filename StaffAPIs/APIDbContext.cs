using Microsoft.EntityFrameworkCore;
using StaffManagement.Models;

namespace StaffAPIs
{
    public class APIDbContext : DbContext,SqlServerDbContext
    {
        public APIDbContext(DbContextOptions<APIDbContext> options) : base(options)
        {
            Staffs = Set<Staff>();
        }

        public DbSet<Staff> Staffs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Staff>().ToTable("Staff");
            

        }

        

    }
    
       

}
