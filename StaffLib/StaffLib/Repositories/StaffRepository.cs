
using Microsoft.EntityFrameworkCore;

using StaffManagement.Models;

namespace StaffManagement.Repositories
{
    public class StaffRepository : IStaffRepository
    {
        private readonly SqlServerDbContext _context;
        public StaffRepository(SqlServerDbContext context)
        {
            _context = context;
        }
        public async Task<Staff> AddStaff(Staff staff )
        {  
            
            _context.Staffs.Add(staff); 
            _context.SaveChanges();
            return staff;
        } 
        public void DeleteStaff(int id)
        {
            var staff = _context.Staffs.Find(id);
            _context.Staffs.Remove(staff);
            _context.SaveChanges();
            
        }


        public IQueryable<Staff> GetQueryable()
        {
            return _context.Staffs.AsQueryable();
        }

        public async Task<Staff> GetStaffById(int id)
        {
            var staff = await GetQueryable().FirstOrDefaultAsync(s => s.Id == id);
            return staff;
            
        }

        public async Task<IEnumerable<Staff>> GetStaffs()
        {
            IEnumerable<Staff> staffList = await _context.Staffs.ToListAsync();
            return staffList;
        }

        public async Task<Staff> UpdateStaff(Staff staff)
        {
            var foundStaff = await GetStaffById(staff.Id);
            if (foundStaff != null)
            {
                foundStaff.Name = staff.Name;
                foundStaff.PhoneNumber = staff.PhoneNumber;
                foundStaff.Department = staff.Department;
                foundStaff.Department = staff.Department;
                _context.SaveChanges();
            }
            return foundStaff;  
        }
        
    }
}
