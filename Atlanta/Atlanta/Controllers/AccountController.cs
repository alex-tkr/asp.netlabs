using System.Security.Claims;

using Atlanta.Services.Interfaces;
using Atlanta.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Atlanta.Controllers;

public class AccountController : Controller
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        this._accountService = accountService;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var response = await this._accountService.Register(model);
            if (!response.HasError)
            {
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(response.Entity!));
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("register", response.Description);
        }
        return View(model);
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        var userIsActive = await this._accountService.Check(model);
        if (ModelState.IsValid)
        {
            if (userIsActive)
            {
                var response = await this._accountService.Login(model);
                if (!response.HasError)
                {
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(response.Entity!));

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("login", response.Description);
            }
            else
            {
                ModelState.AddModelError("login", "Пользователь помечен как неактивный");
            }
        }
        
        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}