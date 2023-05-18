using Atlanta.Domain.Enums;

namespace Atlanta.ViewModel;

public class UserViewModel
{
    public string? Email { get; set; }
    public int Role { get; set; }
    public string? Password { get; set; }
    public bool IsActive { get; set; }
}