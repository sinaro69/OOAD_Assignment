using StaffManagement.Models;

namespace StaffManagement.Repositories
{
    public interface IStaffRepository

    {
        Task<IEnumerable<Staff>> GetStaffs();
        public IQueryable<Staff> GetQueryable();
        Task<Staff> GetStaffById(int id);
        Task<Staff> AddStaff(Staff staff);
        Task<Staff> UpdateStaff(Staff staff);
        void DeleteStaff(int id);
    }
}
