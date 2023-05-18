using Atlanta.DAL.Interfaces;
using Atlanta.Domain.Entity;
using Atlanta.Domain.Utils;
using Atlanta.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Atlanta.DAL.Repositories;

public class StaffRepository : IStaffRepository
{
    private readonly ApplicationDbContext _dbContext;

    public StaffRepository(ApplicationDbContext dbContext)
    {
        this._dbContext = dbContext;
    }


    public async Task<IEnumerable<Staff>> SelectAsync()
    {
        return await this._dbContext.Staff.ToListAsync();
    }

    public async Task<bool> Add(Staff entity)
    {
        entity.IsActive = true;
        entity.IsWait = true;
        var response = await this._dbContext.Staff.AddAsync(entity);
        var result = await this._dbContext.SaveChangesAsync();
        return result >= 1;
    }

    public Task<bool> Delete(Staff entity)
    {
        throw new NotImplementedException();
    }

    public async Task<Staff> GetById(int id)
    {
        var response = await this._dbContext.Staff.FirstOrDefaultAsync(i => i.IdStaff == id);
        return response;
    }

    public async Task<bool> Update(Staff entity)
    {
        var response = await this._dbContext.Staff.FirstOrDefaultAsync(i => i.IdStaff == entity.IdStaff);
        response.NameStaff = entity.NameStaff;
        response.AmountPeople = entity.AmountPeople;
        response.Price = entity.Price;
        response.IdTypeStaff = entity.IdTypeStaff;
        response.Location = entity.Location;
        response.IsActive = entity.IsActive;
        var result = await this._dbContext.SaveChangesAsync();
        return result >= 1;
    }

    public async Task<bool> AddStaffViewModel(StaffViewModel entity)
    {
        var response = await this._dbContext.TypeStaff.FirstOrDefaultAsync(i => i.TypeName == entity.IdTypeStaff);
        if (response == null)
        {
            await this._dbContext.TypeStaff.AddAsync(new TypeStaff() { TypeName = entity.IdTypeStaff });
            await this._dbContext.SaveChangesAsync();
            response = await this._dbContext.TypeStaff.FirstOrDefaultAsync(i => i.TypeName == entity.IdTypeStaff);
        }

        var staff = StaffConvertor.ConvertToStaff(entity, response!.IdTypeStaff);
        
        return await this.Add(staff);
    }

    public async Task<IEnumerable<Staff>> GetWaitStaff()
    {
        var response = await this._dbContext.Staff.ToListAsync();
        var result = response.Where(i => i.IsWait == true);
        return result;
    }

    public async Task<TypeStaff> GetTypeStaff(int idTypeStaff)
    {
        var response = await this._dbContext.TypeStaff.FirstOrDefaultAsync(i => i.IdTypeStaff == idTypeStaff);
        return response;
    }

    public async Task<TypeStaff> GetTypeStaff(string typeStaffName)
    {
        return await this._dbContext.TypeStaff.FirstOrDefaultAsync(i => i.TypeName == typeStaffName);
    }

    public async Task<bool> StaffApprove(int idStaff)
    {
        var response = await this._dbContext.Staff.FirstOrDefaultAsync(i => i.IdStaff == idStaff);
        response.IsWait = false;
        var result = await this._dbContext.SaveChangesAsync();
        return result >= 1;
    }

    public async Task<IEnumerable<Staff>> SelectByUser(int idUser)
    {
        var result = await this._dbContext.Staff.ToListAsync();
        var staffs = result.Where(i => i.IdUser == idUser);
        return staffs;
    }
    
    public async Task Activated(int idStaff)
    {
        var response = await this._dbContext.Staff.FirstOrDefaultAsync(i => i.IdStaff == idStaff);
        response.IsActive = true;
        var result = await this._dbContext.SaveChangesAsync();
        return;
    }

    public async Task<IEnumerable<TypeStaff>> SelectTypeStaff()
    {
        return await this._dbContext.TypeStaff.ToListAsync();
    }

    public async Task Deactivated(int idStaff)
    {
        var response = await this._dbContext.Staff.FirstOrDefaultAsync(i => i.IdStaff == idStaff);
        response.IsActive = false;
        var result = await this._dbContext.SaveChangesAsync();
        return;
    }
}