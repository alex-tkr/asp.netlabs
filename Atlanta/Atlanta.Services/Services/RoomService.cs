using Atlanta.DAL.Interfaces;
using Atlanta.Domain.Entity;
using Atlanta.Domain.Utils;
using Atlanta.Services.Interfaces;

namespace Atlanta.Services.Services;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _repository;

    public RoomService(IRoomRepository repository)
    {
        this._repository = repository;
    }

    public async Task<ISimpleResult<IEnumerable<Room>>> GetAllRoomsAsync()
    {
        var response = await this._repository.SelectAsync();
        return SimpleResult<IEnumerable<Room>>.FromSuccess(response);
    }
}