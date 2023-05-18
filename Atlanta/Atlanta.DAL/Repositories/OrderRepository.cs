using Atlanta.DAL.Interfaces;
using Atlanta.Domain.Entity;
using Atlanta.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Atlanta.DAL.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _dbContext;

    public OrderRepository(ApplicationDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task<IEnumerable<Orders>> SelectAsync()
    {
        return await this._dbContext.Orders.ToListAsync();
    }

    public async Task<bool> Add(Orders entity)
    {
        var response = await this._dbContext.Orders.AddAsync(entity);
        var result = await this._dbContext.SaveChangesAsync();
        return result >= 1;
    }

    public Task<bool> Delete(Orders entity)
    {
        throw new NotImplementedException();
    }

    public Task<Orders> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Update(Orders entity)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ApproveRoom(int idOrder)
    {
        var response = await this._dbContext.Orders.FirstOrDefaultAsync(i => i.IdOrder == idOrder);
        if (response.IdStatusOrder == StatusOrder.ReviewRoom_ReviewStaff)
        {
            response.IdStatusOrder = StatusOrder.YesRoom_ReviewStaff;
        }
        else if (response.IdStatusOrder == StatusOrder.ReviewRoom_NoStaff)
        {
            response.IdStatusOrder = StatusOrder.YesRoom_NoStaff;
        }
        else if (response.IdStatusOrder == StatusOrder.ReviewRoom_YesStaff)
        {
            response.IdStatusOrder = StatusOrder.YesRoom_YesStaff;
            if (response.IsWait == false)
            {
                response.IdStatusOrder = StatusOrder.Accepted;
            }
        }

        await this._dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ApproveStaff(int idOrder)
    {
        var response = await this._dbContext.Orders.FirstOrDefaultAsync(i => i.IdOrder == idOrder);
        if (response.IdStatusOrder == StatusOrder.ReviewRoom_ReviewStaff)
        {
            response.IdStatusOrder = StatusOrder.ReviewRoom_YesStaff;
        }
        else if (response.IdStatusOrder == StatusOrder.NoRoom_ReviewStaff)
        {
            response.IdStatusOrder = StatusOrder.NoRoom_YesStaff;
        }
        else if (response.IdStatusOrder == StatusOrder.YesRoom_ReviewStaff)
        {
            response.IdStatusOrder = StatusOrder.YesRoom_YesStaff;
            if (response.IsWait == false)
            {
                response.IdStatusOrder = StatusOrder.Accepted;
            }
        }
        

        await this._dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UnapproveStaff(int idOrder)
    {
        var response = await this._dbContext.Orders.FirstOrDefaultAsync(i => i.IdOrder == idOrder);
        if (response.IdStatusOrder == StatusOrder.ReviewRoom_ReviewStaff)
        {
            response.IdStatusOrder = StatusOrder.ReviewRoom_NoStaff;
        }
        else if (response.IdStatusOrder == StatusOrder.NoRoom_ReviewStaff)
        {
            response.IdStatusOrder = StatusOrder.NoRoom_NoStaff;
        }
        else if (response.IdStatusOrder == StatusOrder.YesRoom_ReviewStaff)
        {
            response.IdStatusOrder = StatusOrder.YesRoom_NoStaff;
        }

        await this._dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UnapproveRoom(int idOrder)
    {
        var response = await this._dbContext.Orders.FirstOrDefaultAsync(i => i.IdOrder == idOrder);
        if (response.IdStatusOrder == StatusOrder.ReviewRoom_ReviewStaff)
        {
            response.IdStatusOrder = StatusOrder.NoRoom_ReviewStaff;
        }
        else if (response.IdStatusOrder == StatusOrder.ReviewRoom_NoStaff)
        {
            response.IdStatusOrder = StatusOrder.NoRoom_NoStaff;
        }
        else if (response.IdStatusOrder == StatusOrder.ReviewRoom_YesStaff)
        {
            response.IdStatusOrder = StatusOrder.NoRoom_YesStaff;
        }

        await this._dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ApproveAdmin(int idOrder)
    {
        var response = await this._dbContext.Orders.FirstOrDefaultAsync(i => i.IdOrder == idOrder);
        response.IsWait = false;
            response.IdStatusOrder = StatusOrder.Accepted;
        await this._dbContext.SaveChangesAsync();
        return true;
    }
    public async Task<bool> UnapproveAdmin(int idOrder)
    {
        var response = await this._dbContext.Orders.FirstOrDefaultAsync(i => i.IdOrder == idOrder);
        response.IsWait = false;
            response.IdStatusOrder = StatusOrder.Declined;
        await this._dbContext.SaveChangesAsync();
        return true;
    }
}