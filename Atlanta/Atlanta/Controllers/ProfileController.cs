using Atlanta.DAL.Interfaces;
using Atlanta.Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Atlanta.Controllers;

[Authorize]
public class ProfileController : Controller
{
    private readonly IUserRepository _userRepository;

    public ProfileController(IUserRepository userRepository)
    {
        this._userRepository = userRepository;
    }

    [HttpGet]
    public async Task <IActionResult> Index()
    {
        var currentUser = await this._userRepository.GetByEmail(User.Identity.Name);
        return View(currentUser);
    }

    [HttpPost]
    public async Task<IActionResult> Index(Users model)
    {
        await this._userRepository.Update(model);
        var currentUser = await this._userRepository.GetByEmail(User.Identity.Name);
        return View(currentUser);
    }

}