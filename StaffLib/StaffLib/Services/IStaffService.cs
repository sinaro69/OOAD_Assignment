
using StaffManagement.Models;

namespace StaffManagement.Services;

public interface IStaffService
{
    Task<IEnumerable<Staff>> GetStaffs();
    Task<Staff?> GetStaffById(int id);
    Task<Staff> AddStaff(Staff staff);
    Task<Staff> UpdateStaff(Staff staff);
    Task<Staff> DeleteStaff(int id);
}