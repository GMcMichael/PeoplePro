using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PeoplePro.Models
{
    public class Department
    {
        public int Id { get; set; }
        [Display(Name = "Department")]
        public string Name { get; set; }
        [DisplayFormat(NullDisplayText = "No employees")]
        public ICollection<Employee> Employees { get; set; }
        [DisplayFormat(NullDisplayText = "No Rooms")]
        public ICollection<DepartmentRoom> Rooms { get; set; }
        [DisplayFormat(NullDisplayText = "No Buildings")]
        public ICollection<BuildingDepartment> Buildings { get; set; }

        public bool ContainsEmployee(int id)
        {
            foreach (var employee in Employees)
            {
                if (employee.Id == id) return true;
            }
            return false;
        }
        public bool ContainsRoom(int id)
        {
            foreach (var room in Rooms)
            {
                if (room.RoomId == id) return true;
            }
            return false;
        }
        public bool ContainsBuilding(int id)
        {
            foreach (var building in Buildings)
            {
                if (building.BuildingId == id) return true;
            }
            return false;
        }
    }
}
