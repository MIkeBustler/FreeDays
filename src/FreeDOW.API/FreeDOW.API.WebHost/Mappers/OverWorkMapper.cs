using FreeDOW.API.Core.Entities;
using FreeDOW.API.WebHost.Models;

namespace FreeDOW.API.WebHost.Mappers
{
    public static class OverWorkMapper
    {
        public static OverWork MapFromModel(CreateOrEditOverWork cow, Equipment eq, Employee emp, OverWork overwork = null)
        {
            if (null == overwork)
            {
                overwork = new OverWork();
                overwork.Id = Guid.NewGuid();
                overwork.Confirmed = 0;
                overwork.Reminder = (int)(cow.DTEnd - cow.DTStart).TotalMinutes;
            }
            overwork.DTStart = cow.DTStart;
            overwork.DTEnd = cow.DTEnd;
            overwork.Desription = cow.Description;
            overwork.EquipmentId = cow.EquipmentId;
            overwork.EmployeeId = cow.EmployeeId;
            
            overwork.EquipmentDetail = cow.EquipmentDetail;

            return overwork;
        }
    }
}
