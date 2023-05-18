using System.Collections;
using Atlanta.Domain.Entity;
using Atlanta.ViewModel;

namespace Atlanta.DAL.Interfaces;

public interface IStaffRepository : IBaseRepository<Staff>
{
    Task<bool> AddStaffViewModel(StaffViewModel entity);
    Task<IEnumerable<Staff>> GetWaitStaff();
    Task<TypeStaff> GetTypeStaff(int staffIdTypeStaff);
    Task<TypeStaff> GetTypeStaff(string typeStaffName);
    Task<bool> StaffApprove(int idStaff);
    Task<IEnumerable<Staff>> SelectByUser(int idUser);
    Task Deactivated(int idStaff);
    Task Activated(int idStaff);
    Task<IEnumerable<TypeStaff>> SelectTypeStaff();

}