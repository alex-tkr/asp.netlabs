using System.ComponentModel.DataAnnotations;

namespace Atlanta.ViewModel;

public class StaffViewModel
{
    
    public int IdStaff { get; set; }
    [Required]
    [Display(Name = "Название Организации")]
    public string? NameStaff { get; set; }
    [Required]
    [Display(Name = "Количество людей в персонале")]
    public int AmountPeople { get; set; }
    [Required]
    [Display(Name = "Цена")]
    public decimal Price { get; set; }
    [Required]
    [Display(Name = "Местоположение")]
    public string? Location { get; set; }
    public int IdUser { get; set; }
    [Required]
    [Display(Name = "Тип персонала")]
    public string? IdTypeStaff { get; set; }
    public bool IsActive { get; set; }
    public bool IsWait { get; set; }
}