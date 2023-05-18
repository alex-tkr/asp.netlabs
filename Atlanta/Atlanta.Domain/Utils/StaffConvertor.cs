using Atlanta.Domain.Entity;
using Atlanta.ViewModel;

namespace Atlanta.Domain.Utils;

public class StaffConvertor
{
    static public StaffViewModel ConvertToStaffView(Staff entity, string typeStaff)
    {
        var staffView = new StaffViewModel()
        {
            IdStaff = entity.IdStaff,
            NameStaff = entity.NameStaff,
            AmountPeople = entity.AmountPeople,
            Price = entity.Price,
            Location = entity.Location,
            IdTypeStaff = typeStaff,
            IdUser = entity.IdUser,
            IsActive = entity.IsActive,
            IsWait = entity.IsWait
        };
        return staffView;
    }

    static public Staff ConvertToStaff(StaffViewModel entity, int idTypeStaff)
    {
        var staff = new Staff()
        {
            IdStaff = entity.IdStaff,
            NameStaff = entity.NameStaff,
            AmountPeople = entity.AmountPeople,
            Price = entity.Price,
            Location = entity.Location,
            IdTypeStaff = idTypeStaff,
            IdUser = entity.IdUser,
            IsActive = entity.IsActive,
            IsWait = entity.IsWait
        };
        return staff;
    }
}