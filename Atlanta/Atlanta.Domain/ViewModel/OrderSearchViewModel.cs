using System.ComponentModel.DataAnnotations;

namespace Atlanta.ViewModel;

public class OrderSearchViewModel
{
    public int MaxAmountPeople { get; set; }
    public Dictionary<int, string> TypeRoom { get; set; }
    public Dictionary<int, string> TypeStaff { get; set; }
    public int TypeRoomValue { get; set; }
    public int TypeStaffValue { get; set; }
    public DateTime DateOfEvent { get; set; }
    public string LocationValue { get; set; }
    public List<string> Location { get; set; }
    
}