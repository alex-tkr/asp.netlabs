using Atlanta.Domain.Entity;
using Atlanta.ViewModel;

namespace Atlanta.Domain.Utils;

public class RoomConvertor
{
    static public RoomViewModel ConvertToRoomView(Room entity, string typeRoom)
    {
        var roomView = new RoomViewModel()
        {
            IdRoom = entity.IdRoom,
            NameRoom = entity.NameRoom,
            MaxAmountPeople = entity.MaxAmountPeople,
            Price = entity.Price,
            Location = entity.Location,
            IdTypeRoom = typeRoom,
            IdUser = entity.IdUser,
            IsActive = entity.IsActive,
            IsWait = entity.IsWait
        };
        return roomView;
    }

    static public Room ConvertToRoom(RoomViewModel entity, int idTypeRoom)
    {
        var room = new Room()
        {
            IdRoom = entity.IdRoom,
            NameRoom = entity.NameRoom,
            MaxAmountPeople = entity.MaxAmountPeople,
            Price = entity.Price,
            Location = entity.Location,
            IdTypeRoom = idTypeRoom,
            IdUser = entity.IdUser,
            IsActive = entity.IsActive,
            IsWait = entity.IsWait
        };
        return room;
    }
}