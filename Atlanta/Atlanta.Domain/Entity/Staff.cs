using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atlanta.Domain.Entity;

public class Staff
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int IdStaff { get; set; }
    public string? NameStaff { get; set; }
    public int AmountPeople { get; set; }
    public decimal Price { get; set; }
    public string? Location { get; set; }
    public int IdUser { get; set; }
    public int IdTypeStaff { get; set; }
    public bool IsActive { get; set; }
    public bool IsWait { get; set; }
}