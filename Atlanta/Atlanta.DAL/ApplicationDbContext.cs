using Atlanta.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Atlanta.DAL;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}
    
    public DbSet<Room> Room { get; set; }

    public DbSet<Users> Users { get; set; }
    
    public DbSet<TypeRoom> TypeRoom { get; set; }
    
    public DbSet<Staff> Staff { get; set; }
    
    public DbSet<TypeStaff> TypeStaff { get; set; }
    
    public DbSet<Orders> Orders { get; set; }
}