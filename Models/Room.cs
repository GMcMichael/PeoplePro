using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PeoplePro.Models
{
    public class Room
    {
        public int Id { get; set; }
        [Display(Name = "Room")]
        public string Name { get; set; }

        // TODO: Add Department models https://docs.microsoft.com/en-us/ef/core/modeling/relationships#many-to-many
        public List<DepartmentRoom> Departments { get; set; }
        public int BuildingID { get; set; }
        public Building Building { get; set; }
        public List<Employee> Employees { get; set; }
    }
}
