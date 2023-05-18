using System.ComponentModel.DataAnnotations;

namespace Atlanta.ViewModel;

public class LoginViewModel
{
    [Required(ErrorMessage = "Укажите Email")]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }
    [Required(ErrorMessage = "Укажите пароль")]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string? Passwords { get; set; }
}