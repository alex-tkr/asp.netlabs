using System.Net.Security;
using System.Security.Claims;
using Atlanta.DAL.Interfaces;
using Atlanta.Domain.Entity;
using Atlanta.Domain.Enums;
using Atlanta.Domain.Utils;
using Atlanta.Services.Interfaces;
using Atlanta.ViewModel;
namespace Atlanta.Domain.Services;

public class AccountService : IAccountService
{
    private readonly IUserRepository _userRepository;

    public AccountService(IUserRepository userRepository)
    {
        this._userRepository = userRepository;
    }
    
    public async Task<SimpleResult<ClaimsIdentity>> Login(LoginViewModel model)
    {
        try
        {
            var user = await this._userRepository.GetByEmail(model.Email);
            if (user == null)
            {
                return SimpleResult<ClaimsIdentity>.FromError("Пользователь не найден");
            }

            if (user.Passwords != model.Passwords)
            {
                return SimpleResult<ClaimsIdentity>.FromError("Неверный пароль");
            }

            var result = Authenticate(user);
            return SimpleResult<ClaimsIdentity>.FromSuccess(result);
        }
        catch (Exception e)
        {
            return SimpleResult<ClaimsIdentity>.FromError(e.Message);
        }
    }
    
    public async Task<SimpleResult<ClaimsIdentity>> Register(RegisterViewModel model)
    {
        try
        {
            var tryGetUser = await this._userRepository.GetByEmail(model.Email);
            if (tryGetUser != null)
            {
                return SimpleResult<ClaimsIdentity>.FromError("Пользователь с таким Email существует");
            }

            var user = new Users()
            {
                Email = model.Email,
                Passwords = model.Password,
                IdRole = Roles.User
            };
            var resultB = await this._userRepository.Add(user);
            if (!resultB)
            {
                return SimpleResult<ClaimsIdentity>.FromError("Что-то пошло не так :(");
            }

            var registerUser = await this._userRepository.GetByEmail(user.Email);
            var result = Authenticate(registerUser);
            return SimpleResult<ClaimsIdentity>.FromSuccess(result);
        }
        catch (Exception e)
        {
            return SimpleResult<ClaimsIdentity>.FromError(e.Message);
        }
    }

    public async Task<bool> Check(LoginViewModel model)
    {
        var user = await this._userRepository.GetByEmail(model.Email!);
        return user.IsActive;
    }

    private ClaimsIdentity Authenticate(Users user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, user.IdRole.ToString())
        };
        return new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);
    }
}