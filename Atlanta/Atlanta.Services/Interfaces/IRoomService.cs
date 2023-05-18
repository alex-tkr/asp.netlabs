using Atlanta.Domain.Entity;
using Atlanta.Domain.Utils;

namespace Atlanta.Services.Interfaces;

public interface IRoomService
{
    Task<ISimpleResult<IEnumerable<Room>>> GetAllRoomsAsync();
}