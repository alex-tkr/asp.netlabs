using Atlanta.Domain.Entity;

namespace Atlanta.DAL.Interfaces;

public interface IOrderRepository : IBaseRepository<Orders>
{
    Task<bool> ApproveRoom(int idOrder);
    Task<bool> ApproveStaff(int idOrder);
    Task<bool> UnapproveStaff(int idOrder);
    Task<bool> UnapproveRoom(int idOrder);
    Task<bool> ApproveAdmin(int idOrder);
    Task<bool> UnapproveAdmin(int idOrder);
}