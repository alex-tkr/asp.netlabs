using System.Security.Claims;
using Atlanta.Domain.Utils;
using Atlanta.ViewModel;

namespace Atlanta.Services.Interfaces;

public interface IAccountService
{
    Task<SimpleResult<ClaimsIdentity>> Login(LoginViewModel model);

    Task<SimpleResult<ClaimsIdentity>> Register(RegisterViewModel model);

    Task<bool> Check(LoginViewModel model);
}