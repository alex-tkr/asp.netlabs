using System.Diagnostics;
using Atlanta.DAL.Interfaces;
using Atlanta.Domain.Entity;
using Atlanta.Domain.Enums;
using Atlanta.Domain.Utils;
using Microsoft.AspNetCore.Mvc;
using Atlanta.Models;
using Atlanta.ViewModel;
using Microsoft.Identity.Client;

namespace Atlanta.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUserRepository _repository;
    private readonly IStaffRepository _staffRepository;
    private readonly IRoomRepository _roomRepository;

    public HomeController(ILogger<HomeController> logger, IUserRepository repository, IStaffRepository staffRepository, IRoomRepository roomRepository)
    {
        this._logger = logger;
        this._repository = repository;
        this._staffRepository = staffRepository;
        this._roomRepository = roomRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var staffRoom = new StaffRoomViewModel();
        var rooms = (await this._roomRepository.SelectAsync()).ToList().Where(i => i.IsActive && i.IsWait == false).ToList();
        var staffs = (await this._staffRepository.SelectAsync()).ToList().Where(i => i.IsActive && i.IsWait == false).ToList();
        
        List<RoomViewModel> roomViewModels = new List<RoomViewModel>();
        foreach (var room in rooms)
        {
            var typeRoom = await this._roomRepository.GetTypeRoom(room.IdTypeRoom);
            roomViewModels.Add(RoomConvertor.ConvertToRoomView(room, typeRoom.TypeName));
        }
        List<StaffViewModel> staffViewModels = new List<StaffViewModel>();
        foreach (var staff in staffs)
        {
            var typeStaff = await this._staffRepository.GetTypeStaff(staff.IdTypeStaff);
            staffViewModels.Add(StaffConvertor.ConvertToStaffView(staff, typeStaff.TypeName));
        }

        staffRoom.Rooms = roomViewModels;
        staffRoom.Staffs = staffViewModels;
        return View(staffRoom);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}