using Atlanta.DAL.Interfaces;
using Atlanta.Domain.Entity;
using Atlanta.Domain.Enums;
using Atlanta.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace Atlanta.Controllers;

[Authorize]
public class OrdersController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IStaffRepository _staffRepository;

    public OrdersController(IUserRepository userRepository, IOrderRepository orderRepository, IRoomRepository roomRepository, IStaffRepository staffRepository)
    {
        this._userRepository = userRepository;
        this._orderRepository = orderRepository;
        this._roomRepository = roomRepository;
        this._staffRepository = staffRepository;
    }

    [HttpGet]
    public async Task<IActionResult> NewOrder()
    {
        var orderSearch = new OrderSearchViewModel();

        orderSearch.TypeRoom = new Dictionary<int, string>();
        orderSearch.TypeStaff = new Dictionary<int, string>();
        orderSearch.Location = new List<string>();
        var rooms = await this._roomRepository.SelectAsync();
        var staffs = await this._staffRepository.SelectAsync();
        rooms = rooms.Where(i => i.IsActive == true && i.IsWait == false);
        staffs = staffs.Where(i => i.IsActive == true && i.IsWait == false);
        var typeRoomListId = rooms.Select(i => i.IdTypeRoom);
        var typeStaffListId = staffs.Select(i => i.IdTypeStaff);

        var typeRoomList1 = new List<TypeRoom>();
        foreach (var id in typeRoomListId)
        {
            typeRoomList1.Add(await this._roomRepository.GetTypeRoom(id));
        }

        var typeRoomList = typeRoomList1.Distinct();
        var typeStaffList1 = new List<TypeStaff>();
        foreach (var id in typeStaffListId)
        {
            typeStaffList1.Add(await this._staffRepository.GetTypeStaff(id));
        }

        var typeStaffList = typeStaffList1.Distinct();
        var generalLocation =
            from r in rooms
            from s in staffs
            where r.Location == s.Location
            select r.Location;
        orderSearch.Location = generalLocation.ToList().Distinct().ToList();
        foreach (var typeRoom in typeRoomList)
        {
            orderSearch.TypeRoom.Add(typeRoom.IdTypeRoom, typeRoom.TypeName);
        }
        foreach (var typeStaff in typeStaffList)
        {
            orderSearch.TypeStaff.Add(typeStaff.IdTypeStaff, typeStaff.TypeName);
        }
        return View(orderSearch);
    }

    
    [HttpGet]
    public async Task<IActionResult> SearchForOrder(OrderSearchViewModel model)
    {
        var rooms = await this._roomRepository.SelectAsync();
        var room1 = rooms.Where(i => i.IdTypeRoom == model.TypeRoomValue && i.MaxAmountPeople >= model.MaxAmountPeople && i.Location == model.LocationValue).ToList();
        var staffs = await this._staffRepository.SelectAsync();
        var staff1 = staffs.Where(i => i.IdTypeStaff == model.TypeStaffValue && i.Location == model.LocationValue).ToList();

        var ordersWithDate = await this._orderRepository.SelectAsync();
        ordersWithDate = ordersWithDate.Where(i => i.DateOfEvent == model.DateOfEvent);
        foreach (var o in ordersWithDate)
        {
            var rom = room1.Where(i => i.IdRoom == o.IdRoom).ToList();
            if (rom.Any())
            {
                room1.Remove(rom.First());
            }

            var saf = staff1.Where(i => i.IdStaff == o.IdStaff).ToList();
            if (saf.Any())
            {
                staff1.Remove(saf.First());
            }
        }
        var order = new SecondOrderViewModel();
        order.Rooms = room1;
        order.Staffs = staff1;
        order.DateOfEvent = model.DateOfEvent;
        order.AmountPeople = model.MaxAmountPeople;
        order.Location = model.LocationValue;
        return View(order);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(SecondOrderViewModel model)
    {
        var order = new Orders();
        var room = await this._roomRepository.GetById(model.CurrentRoom);
        var staff = await this._staffRepository.GetById(model.CurrentStaff);
        order.IdStaff = staff.IdStaff;
        order.IdRoom = room.IdRoom;
        order.AllPrice = staff.Price + room.Price;
        order.DateOfEvent = model.DateOfEvent;
        var user = await this._userRepository.GetByEmail(User.Identity.Name);
        order.IdUser = user.IdUser;
        order.IdStatusOrder = StatusOrder.ReviewRoom_ReviewStaff;
        order.IsWait = true;
        await this._orderRepository.Add(order);
        return RedirectToAction("Index", "Home");
    }


    [HttpGet]
    public async Task<IActionResult> MyOrders()
    {
        var userOrder = await this._userRepository.GetByEmail(User.Identity.Name);
        var rooms = (await this._roomRepository.SelectAsync()).ToList()
            .Where(i => i.IsActive && i.IsWait == false).ToList();
        var response = (await this._orderRepository.SelectAsync()).ToList().Where(i => i.IdUser == userOrder.IdUser);
        List<OrderForUser> listForUser = new List<OrderForUser>();
        var staffs = (await this._staffRepository.SelectAsync()).ToList();
        foreach (var order in response)
        {
            var orderView = new OrderForUser();
            orderView.RoomView = rooms.FirstOrDefault(i => i.IdRoom == order.IdRoom).NameRoom;
            orderView.StaffView = staffs.FirstOrDefault(i => i.IdStaff == order.IdStaff).NameStaff;
            orderView.AllPrice = order.AllPrice;
            
            orderView.NameUser = userOrder.Email;
            orderView.IdOrder = order.IdOrder;
            orderView.IdStatusOrder = order.IdStatusOrder;
            orderView.IsWait = order.IsWait;
            listForUser.Add(orderView);
        }

        return View(listForUser);
    }
}