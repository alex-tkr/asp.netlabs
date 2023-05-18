using System.Windows.Markup;
using Atlanta.DAL.Interfaces;
using Atlanta.Domain.Entity;
using Atlanta.Domain.Utils;
using Atlanta.Services.Interfaces;
using Atlanta.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Atlanta.Controllers;

[Authorize(Roles = "RoomMan")]
public class RoomController : Controller
{
    private readonly IRoomRepository _roomRepository;
    private readonly IUserRepository _userRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IStaffRepository _staffRepository;

    public RoomController(IRoomRepository roomRepository, IUserRepository userRepository,
        IOrderRepository orderRepository, IStaffRepository staffRepository)
    {
        this._roomRepository = roomRepository;
        this._userRepository = userRepository;
        this._orderRepository = orderRepository;
        this._staffRepository = staffRepository;
    }

    [HttpGet]
    public IActionResult AddRoom()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddRoom(RoomViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await this._userRepository.GetByEmail(User.Identity.Name);
            model.IdUser = user.IdUser;
            await this._roomRepository.AddRoomViewModel(model);
            return RedirectToAction("MyRooms", "Room");
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> MyRooms()
    {
        var user = await this._userRepository.GetByEmail(User.Identity.Name);
        var result = await this._roomRepository.SelectByUser(user.IdUser);
        return View(result);
    }

    [HttpGet]
    public async Task<IActionResult> MyRoomById(int room1)
    {
        var room = await this._roomRepository.GetById(room1);

        var typeRoom = await this._roomRepository.GetTypeRoom(room.IdTypeRoom);
        var roomView = new RoomViewModel()
        {
            NameRoom = room.NameRoom,
            IdTypeRoom = typeRoom.TypeName,
            Location = room.Location,
            IdUser = room.IdUser,
            MaxAmountPeople = room.MaxAmountPeople,
            Price = room.Price,
            IdRoom = room.IdRoom
        };

        return View(roomView);
    }

    [HttpGet]
    public async Task<IActionResult> MyRoomEdit(int idRoom)
    {
        var response = await this._roomRepository.GetById(idRoom);
        var typeRoom = await this._roomRepository.GetTypeRoom(response.IdTypeRoom);
        var roomView = RoomConvertor.ConvertToRoomView(response, typeRoom.TypeName);
        return View(roomView);
    }

    [HttpPost]
    public async Task<IActionResult> MyRoomEdit(RoomViewModel model)
    {
        var typeRoom = await this._roomRepository.GetTypeRoom(model.IdTypeRoom);

        var room = RoomConvertor.ConvertToRoom(model, typeRoom.IdTypeRoom);
        var response = await this._roomRepository.Update(room);
        return RedirectToAction("MyRooms");
    }

    [HttpGet]
    public async Task<IActionResult> MyOrders()
    {
        var user = await this._userRepository.GetByEmail(User.Identity.Name);
        var myRooms = (await this._roomRepository.SelectByUser(user.IdUser)).ToList()
            .Where(i => i.IsActive && i.IsWait == false).ToList();
        var response = (await this._orderRepository.SelectAsync()).ToList()
            .IntersectBy(myRooms.Select(i => i.IdRoom), i => i.IdRoom).ToList();
        List<OrderForUser> listForUser = new List<OrderForUser>();
        var staffs = await this._staffRepository.SelectAsync();
        foreach (var order in response)
        {
            var orderView = new OrderForUser();
            orderView.RoomView = myRooms.FirstOrDefault(i => i.IdRoom == order.IdRoom).NameRoom;
            orderView.StaffView = staffs.FirstOrDefault(i => i.IdStaff == order.IdStaff).NameStaff;
            orderView.AllPrice = order.AllPrice;
            var userOrder = await this._userRepository.GetById(order.IdUser);
            orderView.NameUser = userOrder.Email;
            orderView.IdOrder = order.IdOrder;
            orderView.IdStatusOrder = order.IdStatusOrder;
            listForUser.Add(orderView);
        }

        return View(listForUser);
    }

    [HttpPost]
    public async Task<IActionResult> ApproveOrder(int idOrder)
    {
        var order = await this._orderRepository.ApproveRoom(idOrder);
        return RedirectToAction("MyOrders", "Room");
    }
    [HttpPost]
    public async Task<IActionResult> UnapproveOrder(int idOrder)
    {
        var order = await this._orderRepository.UnapproveRoom(idOrder);
        return RedirectToAction("MyOrders", "Room");
    }
    
    
}