using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Atlanta.Domain.Enums;

namespace Atlanta.Domain.Entity;

public class Orders
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int IdOrder { get; set; }
    public DateTime DateOfEvent { get; set; }
    public decimal AllPrice { get; set; }
    public int IdStaff { get; set; }
    public int IdRoom { get; set; }
    public int IdUser { get; set; }
    public StatusOrder IdStatusOrder { get; set; }
    public bool IsWait { get; set; }
}