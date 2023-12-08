
using StaffManagement.Models;
using StaffManagement.Repositories;

namespace StaffManagement.Services;

public class StaffService : IStaffService
{
    private readonly IStaffRepository _staffRepository;

    public StaffService(IStaffRepository staffRepository)
    {
        _staffRepository = staffRepository;
    }

    public async Task<IEnumerable<Staff>> GetStaffs()
    {
        var staffs = await _staffRepository.GetStaffs();
        return staffs;
    }
    
    public async Task<Staff?> GetStaffById(int id)
    {
        var entity = await _staffRepository.GetStaffById(id);
        return entity;
    }
   

    public async Task<Staff> AddStaff(Staff staff)
    {
        
        var result = await _staffRepository.AddStaff(staff);
        return result;
    }

    public async Task<Staff> UpdateStaff( Staff staff)
    {
        var result = await _staffRepository.UpdateStaff(staff);
        return result;
    }
    
    public Task<Staff> DeleteStaff(int id)
    {
        _staffRepository.DeleteStaff(id);
        return Task.FromResult(new Staff());
    }
}