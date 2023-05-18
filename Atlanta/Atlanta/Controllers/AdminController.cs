using Atlanta.DAL;
using Atlanta.DAL.Interfaces;
using Atlanta.Domain.Entity;
using Atlanta.Domain.Utils;
using Atlanta.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace Atlanta.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IStaffRepository _staffRepository;
    private readonly IOrderRepository _orderRepository;

    public AdminController(IUserRepository userRepository, IRoomRepository roomRepository, IStaffRepository staffRepository, IOrderRepository orderRepository)
    {
        this._userRepository = userRepository;
        this._roomRepository = roomRepository;
        this._staffRepository = staffRepository;
        this._orderRepository = orderRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var listUser = await this._userRepository.SelectAsync();
        return View(listUser);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string user)
    {
        var user1 = await this._userRepository.GetByEmail(user);
        return View(user1);
    }
    
    [HttpPost]
    public async Task<IActionResult> Edit(Users model)
    {
        await this._userRepository.Update(model);
        return RedirectToAction("Index", "Admin");
    }
    
    [HttpGet]
    public async Task<IActionResult> Delete(string user)
    {
        var user1 = await this._userRepository.GetByEmail(user);
        await this._userRepository.Delete(user1);
        return RedirectToAction("Index", "Admin");
    }

    [HttpGet]
    public async Task<IActionResult> RoomApprove()
    {
        var response = await this._roomRepository.GetWaitRoom();
        var roomsView = new List<RoomViewModel>();
        foreach (var room in response)
        {
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
            roomsView.Add(roomView);
        }
        return View(roomsView);
    }

     [HttpPost]
     public async Task<IActionResult> RoomApprove(int idRoom)
     {
         var result = await this._roomRepository.RoomApprove(idRoom);
         var response = await this._roomRepository.GetWaitRoom();
         var roomsView = new List<RoomViewModel>();
         foreach (var room in response)
         {
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
             roomsView.Add(roomView);
         }

         return View(roomsView);
     }
     
     [HttpGet]
     public async Task<IActionResult> StaffApprove()
     {
         var response = await this._staffRepository.GetWaitStaff();
         var staffsView = new List<StaffViewModel>();
         foreach (var staff in response)
         {
             var typeStaff = await this._staffRepository.GetTypeStaff(staff.IdTypeStaff);
             var staffView = StaffConvertor.ConvertToStaffView(staff, typeStaff.TypeName);
             staffsView.Add(staffView);
         }
         return View(staffsView);
     }

     [HttpPost]
     public async Task<IActionResult> StaffApprove(int idStaff)
     {
         var result = await this._staffRepository.StaffApprove(idStaff);
         var response = await this._staffRepository.GetWaitStaff();
         var staffsView = new List<StaffViewModel>();
         foreach (var staff in response)
         {
             var typeStaff = await this._staffRepository.GetTypeStaff(staff.IdTypeStaff);
             var staffView = StaffConvertor.ConvertToStaffView(staff, typeStaff.TypeName);
             staffsView.Add(staffView);
         }

         return View(staffsView);
     }

     [HttpGet]
     public async Task<IActionResult> ViewRoom()
     {
         var response = await this._roomRepository.SelectAsync();
         return View(response);
     }
     [HttpPost]
     public async Task<IActionResult> RoomDeactivate(int idRoom)
     {
         await this._roomRepository.Deactivated(idRoom);
         return Redirect("ViewRoom");
     }
     [HttpPost]
     public async Task<IActionResult> RoomActivate(int idRoom)
     {
         await this._roomRepository.Activated(idRoom);
         return Redirect("ViewRoom");
     }
     
     [HttpGet]
     public async Task<IActionResult> ViewStaff()
     {
         var response = await this._staffRepository.SelectAsync();
         return View(response);
     }
     [HttpPost]
     public async Task<IActionResult> StaffDeactivate(int idStaff)
     {
         await this._staffRepository.Deactivated(idStaff);
         return Redirect("ViewStaff");
     }
     [HttpPost]
     public async Task<IActionResult> StaffActivate(int idStaff)
     {
         await this._staffRepository.Activated(idStaff);
         return Redirect("ViewStaff");
     }

     [HttpGet]
     public async Task<IActionResult> OrdersUser()
     {
         var rooms = (await this._roomRepository.SelectAsync()).ToList()
             .Where(i => i.IsActive && i.IsWait == false).ToList();
         var response = (await this._orderRepository.SelectAsync()).ToList();
         List<OrderForUser> listForUser = new List<OrderForUser>();
         var staffs = (await this._staffRepository.SelectAsync()).ToList();
         foreach (var order in response)
         {
             var orderView = new OrderForUser();
             orderView.RoomView = rooms.FirstOrDefault(i => i.IdRoom == order.IdRoom).NameRoom;
             orderView.StaffView = staffs.FirstOrDefault(i => i.IdStaff == order.IdStaff).NameStaff;
             orderView.AllPrice = order.AllPrice;
             var userOrder = await this._userRepository.GetById(order.IdUser);
             orderView.NameUser = userOrder.Email;
             orderView.IdOrder = order.IdOrder;
             orderView.IdStatusOrder = order.IdStatusOrder;
             orderView.IsWait = order.IsWait;
             listForUser.Add(orderView);
         }

         return View(listForUser);
     }

     [HttpPost]
     public async Task<IActionResult> ApproveOrder(int idOrder)
     {
         var response = await this._orderRepository.ApproveAdmin(idOrder);
         return RedirectToAction("OrdersUser", "Admin");
     }
     [HttpPost]
     public async Task<IActionResult> UnapproveOrder(int idOrder)
     {
         var response = await this._orderRepository.UnapproveAdmin(idOrder);
         return RedirectToAction("OrdersUser", "Admin");
     }

}