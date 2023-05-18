using System.Diagnostics;
using Atlanta.DAL.Interfaces;
using Atlanta.Domain.Entity;
using Atlanta.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Atlanta.DAL.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UserRepository(ApplicationDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task<IEnumerable<Users>> SelectAsync()
    {
        var result = await this._dbContext.Users.ToListAsync();
        return result;
    }

    public async Task<bool> Add(Users entity)
    {
        entity.IdRole = Roles.User;
        var response = await this._dbContext.Users.AddAsync(entity);
        var result = await this._dbContext.SaveChangesAsync();
        if (result == 1)
        {
            return true;
        }

        return false;
    }

    public async Task<bool> Delete(Users entity)
    {
        var user = await this._dbContext.Users.FirstOrDefaultAsync(i => i.IdUser == entity.IdUser);
        user!.IsActive = false;
        var result = await this._dbContext.SaveChangesAsync();
        return result >= 1;
    }

    public async Task<Users> GetById(int id)
    {
        var response = await this._dbContext.Users.FirstOrDefaultAsync(i => i.IdUser == id);
        return response;
    }

    public async Task<bool> Update(Users user)
    {
        var dbUser = await this._dbContext.Users.FirstOrDefaultAsync(i => i.IdUser == user.IdUser);
        if (dbUser == null)
        {
            return false;
        }
        dbUser.Age = user.Age;
        dbUser.Name = user.Name;
        dbUser.Phone = user.Phone;
        if (user.IdRole != 0)
        {
            dbUser.IdRole = user.IdRole;
        }
            
        dbUser.IsActive = user.IsActive;
        if (user.Passwords != null)
        {
            dbUser.Passwords = user.Passwords;
        }
        var result = await this._dbContext.SaveChangesAsync();
        return result == 1;
    }

    public async Task<Users> GetByEmail(string email)
    {
        var us1 = await this._dbContext.Users.FirstOrDefaultAsync(i => i.Email == email);
        return us1;
    }
}