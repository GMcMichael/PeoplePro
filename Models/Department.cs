using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PeoplePro.Models
{
    public class Department
    {
        public int Id { get; set; }
        [Display(Name = "Department")]
        public string Name { get; set; }

        public List<Employee> Employees { get; set; }
        public List<DepartmentRoom> Rooms { get; set; }
        public List<BuildingDepartment> Buildings { get; set; }
    }
}
