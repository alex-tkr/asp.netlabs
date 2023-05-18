using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Atlanta.Domain.Enums;

namespace Atlanta.Domain.Entity;

public class Users
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int IdUser { get; set; }
    public string? Name { get; set; }
    public int? Age { get; set; }
    public int? Phone { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; } = true;
    public Roles IdRole { get; set; }
    public string? Passwords { get; set; }
}