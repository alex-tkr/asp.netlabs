using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atlanta.Domain.Entity;

public class TypeRoom
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int IdTypeRoom { get; set; }
    public string TypeName { get; set; }
    public string? Description { get; set; }
}