using Atlanta.Domain.Enums;

namespace Atlanta.ViewModel;

public class OrderForUser
{
    public string RoomView { get; set; }
    public string StaffView { get; set; }
    public decimal AllPrice { get; set; }
    public string NameUser { get; set; }
    public int IdOrder { get; set; }
    public StatusOrder IdStatusOrder { get; set; }
    public bool IsWait { get; set; }
}