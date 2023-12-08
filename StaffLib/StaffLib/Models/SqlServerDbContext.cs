using Microsoft.EntityFrameworkCore;

namespace StaffManagement.Models
{
    public interface SqlServerDbContext 
    {
        
        public DbSet<Staff> Staffs { get; set; }

        int SaveChanges();
        
    }
}
