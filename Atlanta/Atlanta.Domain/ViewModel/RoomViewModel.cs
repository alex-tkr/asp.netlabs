using System.ComponentModel.DataAnnotations;

namespace Atlanta.ViewModel;

public class RoomViewModel
{
    public int IdRoom { get; set; }
    [Required]
    [Display(Name = "Название помещения")]
    public string? NameRoom { get; set; }
    [Required]
    [Display(Name = "Максимальное количество людей")]
    public int MaxAmountPeople { get; set; }
    [Required]
    [Display(Name = "Цена")]
    public decimal Price { get; set; }
    [Required]
    [Display(Name = "Местонахождение")]
    public string? Location { get; set; }
    [Required]
    [Display(Name = "Тип помещения")]
    public string IdTypeRoom { get; set; }
    public int IdUser { get; set; }
    public bool IsActive { get; set; }
    public bool IsWait { get; set; }


}