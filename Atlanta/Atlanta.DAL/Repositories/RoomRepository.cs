using Atlanta.DAL.Interfaces;
using Atlanta.Domain.Entity;
using Atlanta.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Atlanta.DAL.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly ApplicationDbContext _dbContext;

    public RoomRepository(ApplicationDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    #region Methods
    
    public async Task<IEnumerable<Room>> SelectAsync()
    {
        return await this._dbContext.Room.ToListAsync();
    }

    public async Task<bool> Add(Room entity)
    {
        var response = await this._dbContext.Room.AddAsync(entity);
        var result = await this._dbContext.SaveChangesAsync();
        
        return result >= 1;
    }

    public async Task<bool> Delete(Room entity)
    {
        throw new NotImplementedException();
    }

    public async Task<Room> GetById(int id)
    {
        return await this._dbContext.Room.FirstOrDefaultAsync(i => i.IdRoom == id);
    }

    public async Task<bool> Update(Room entity)
    {
        var response = await this._dbContext.Room.FirstOrDefaultAsync(i => i.IdRoom == entity.IdRoom);
        response!.IsActive = entity.IsActive;
        response.MaxAmountPeople = entity.MaxAmountPeople;
        response.NameRoom = entity.NameRoom;
        response.Price = entity.Price;
        response.Location = entity.Location;
        var result = await this._dbContext.SaveChangesAsync();
        return result >= 1;
    }

    public Room GetByName(string name)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Room> GetRoomsByUserId(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> AddRoomViewModel(RoomViewModel entity)
    {
        var response = await this._dbContext.TypeRoom.FirstOrDefaultAsync(i => i.TypeName == entity.IdTypeRoom);
        if (response == null)
        {
            await this._dbContext.TypeRoom.AddAsync(new TypeRoom() { TypeName = entity.IdTypeRoom });
            await this._dbContext.SaveChangesAsync();
            response = await this._dbContext.TypeRoom.FirstOrDefaultAsync(i => i.TypeName == entity.IdTypeRoom);
        }

        var room = new Room();
        room.Location = entity.Location;
        room.Price = entity.Price;
        room.IsActive = true;
        room.IsWait = true;
        room.NameRoom = entity.NameRoom;
        room.IdTypeRoom = response.IdTypeRoom;
        room.IdUser = entity.IdUser;
        room.MaxAmountPeople = entity.MaxAmountPeople;
        
        return await this.Add(room);
    }

    public async Task<IEnumerable<Room>> SelectByUser(int idUser)
    {
        var result = await this._dbContext.Room.ToListAsync();
        var rooms = result.Where(i => i.IdUser == idUser);
        return rooms;
    }

    public async Task<TypeRoom> GetTypeRoom(int id)
    {
        return await this._dbContext.TypeRoom.FirstOrDefaultAsync(i => i.IdTypeRoom == id);
    }

    public async Task<TypeRoom> GetTypeRoom(string typeName)
    {
        return await this._dbContext.TypeRoom.FirstOrDefaultAsync(i => i.TypeName == typeName);

    }

    public async Task<IEnumerable<Room>> GetWaitRoom()
    {
        var response = await this._dbContext.Room.ToListAsync();
        var result = response.Where(i => i.IsWait == true);
        return result;
    }

    public async Task<bool> RoomApprove(int IdRoom)
    {
        var response = await this._dbContext.Room.FirstOrDefaultAsync(i => i.IdRoom == IdRoom);
        response.IsWait = false;
        var result = await this._dbContext.SaveChangesAsync();
        return result >= 1;
    }

    public async Task Activated(int idRoom)
    {
        var response = await this._dbContext.Room.FirstOrDefaultAsync(i => i.IdRoom == idRoom);
        response.IsActive = true;
        var result = await this._dbContext.SaveChangesAsync();
        return;
    }
    
    public async Task Deactivated(int idRoom)
    {
        var response = await this._dbContext.Room.FirstOrDefaultAsync(i => i.IdRoom == idRoom);
        response.IsActive = false;
        var result = await this._dbContext.SaveChangesAsync();
        return;
    }

    public async Task<IEnumerable<TypeRoom>> SelectTypeRoom()
    {
        return await this._dbContext.TypeRoom.ToListAsync();
    }

    #endregion
}