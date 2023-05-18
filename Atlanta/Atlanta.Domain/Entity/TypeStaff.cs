using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atlanta.Domain.Entity;

public class TypeStaff
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int IdTypeStaff { get; set; }
    public string? TypeName { get; set; }
    public string? Description { get; set; }
}