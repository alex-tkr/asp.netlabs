using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atlanta.Domain.Entity;

public class Room
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int IdRoom { get; set; }
    public string? NameRoom { get; set; }
    public int MaxAmountPeople { get; set; }
    public decimal Price { get; set; }
    public string? Location { get; set; }
    public int IdTypeRoom { get; set; }
    public int IdUser { get; set; }
    public bool IsActive { get; set; }
    public bool IsWait { get; set; }
}