using Atlanta.DAL.Interfaces;
using Atlanta.Domain.Utils;
using Atlanta.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Atlanta.Controllers;

[Authorize(Roles = "StaffMan")]
public class StaffController : Controller
{
    private readonly IStaffRepository _staffRepository;
    private readonly IUserRepository _userRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IRoomRepository _roomRepository;

    public StaffController(IStaffRepository staffRepository, IUserRepository userRepository, IOrderRepository orderRepository, IRoomRepository roomRepository)
    {
        this._staffRepository = staffRepository;
        this._userRepository = userRepository;
        this._orderRepository = orderRepository;
        this._roomRepository = roomRepository;
    }


    [HttpGet]
    public IActionResult AddStaff()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddStaff(StaffViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await this._userRepository.GetByEmail(User.Identity.Name);
            model.IdUser = user.IdUser;
            await this._staffRepository.AddStaffViewModel(model);
            return RedirectToAction("MyStaffs", "Staff");
        }
        return View(model);
    }
    
    
    [HttpGet]
    public async Task<IActionResult> MyStaffById(int idStaff)
    {
        var staff = await this._staffRepository.GetById(idStaff);
        
        var typeStaff = await this._staffRepository.GetTypeStaff(staff.IdTypeStaff);
        var staffView = StaffConvertor.ConvertToStaffView(staff, typeStaff.TypeName);
        return View(staffView);
    }
    
    [HttpGet]
    public async Task<IActionResult> MyStaffEdit(int idStaff)
    {
        var response = await this._staffRepository.GetById(idStaff);
        var typeStaff = await this._staffRepository.GetTypeStaff(response.IdTypeStaff);
        var staffView = StaffConvertor.ConvertToStaffView(response, typeStaff.TypeName);
        return View(staffView);
    }
    
    [HttpGet]
    public async Task<IActionResult> MyStaffs()
    {
        var user = await this._userRepository.GetByEmail(User.Identity.Name);
        var result = await this._staffRepository.SelectByUser(user.IdUser);
        return View(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> MyStaffEdit(StaffViewModel model)
    {
        var typeRoom = await this._staffRepository.GetTypeStaff(model.IdTypeStaff);

        var staff = StaffConvertor.ConvertToStaff(model, typeRoom.IdTypeStaff);
        var response = await this._staffRepository.Update(staff);
        return RedirectToAction("MyStaffs");
    }
    
    [HttpGet]
    public async Task<IActionResult> MyOrders()
    {
        var user = await this._userRepository.GetByEmail(User.Identity.Name);
        var myStaffs = (await this._staffRepository.SelectByUser(user.IdUser)).ToList()
            .Where(i => i.IsActive && i.IsWait == false).ToList();
        var response = (await this._orderRepository.SelectAsync()).ToList()
            .IntersectBy(myStaffs.Select(i => i.IdStaff), i => i.IdStaff).ToList();
        List<OrderForUser> listForUser = new List<OrderForUser>();
        var rooms = await this._roomRepository.SelectAsync();
        foreach (var order in response)
        {
            var orderView = new OrderForUser();
            orderView.StaffView = myStaffs.FirstOrDefault(i => i.IdStaff == order.IdStaff).NameStaff;
            orderView.RoomView = rooms.FirstOrDefault(i => i.IdRoom == order.IdRoom).NameRoom;
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
        var order = await this._orderRepository.ApproveStaff(idOrder);
        return RedirectToAction("MyOrders", "Staff");
    }
    [HttpPost]
    public async Task<IActionResult> UnapproveOrder(int idOrder)
    {
        var order = await this._orderRepository.UnapproveStaff(idOrder);
        return RedirectToAction("MyOrders", "Staff");
    }
}