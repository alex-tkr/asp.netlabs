using Atlanta.Domain.Entity;
using Atlanta.ViewModel;

namespace Atlanta.DAL.Interfaces;

public interface IRoomRepository : IBaseRepository<Room>
{
    Room GetByName(string name);
    IEnumerable<Room> GetRoomsByUserId(int id);
    Task<bool> AddRoomViewModel(RoomViewModel model);
    Task<IEnumerable<Room>> SelectByUser(int idUser);
    Task<TypeRoom> GetTypeRoom(int id);
    Task<TypeRoom> GetTypeRoom(string typeName);
    Task<IEnumerable<Room>> GetWaitRoom();
    Task<bool> RoomApprove(int IdRoom);
    Task Activated(int idRoom);
    Task Deactivated(int idRoom);
    Task<IEnumerable<TypeRoom>> SelectTypeRoom();
}