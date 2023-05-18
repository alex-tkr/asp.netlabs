using Atlanta.Domain.Entity;

namespace Atlanta.DAL.Interfaces;

public interface IUserRepository : IBaseRepository<Users>
{
    Task<Users> GetByEmail(string email);
}