using Atlanta.Domain.Entity;

namespace Atlanta.ViewModel;

public class SecondOrderViewModel
{
    public List<Room> Rooms { get; set; }
    public List<Staff> Staffs { get; set; }
    public int CurrentRoom { get; set; }
    public int CurrentStaff { get; set; }
    public DateTime DateOfEvent { get; set; }
    public int AmountPeople { get; set; }
    public string Location { get; set; }
}