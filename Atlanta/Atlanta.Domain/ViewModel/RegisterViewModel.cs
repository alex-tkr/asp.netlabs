using System.ComponentModel.DataAnnotations;

namespace Atlanta.ViewModel;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Введите Email")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Введите пароль")]
    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "Пароль должен быть больше 6 символов")]
    public string Password { get; set; }
    
    [Required(ErrorMessage = "Подтвердите пароль")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    public string ConfirmedPassword { get; set; }
}