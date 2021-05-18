using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PeoplePro.Models
{
    public class Room
    {
        public int Id { get; set; }
        [Display(Name = "Room")]
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50)]
        public string Name { get; set; }

        // TODO: Add Department models https://docs.microsoft.com/en-us/ef/core/modeling/relationships#many-to-many
        [DisplayFormat(NullDisplayText = "No Departments")]
        public ICollection<DepartmentRoom> Departments { get; set; }
        public Nullable<int> BuildingID { get; set; }
        [DisplayFormat(NullDisplayText = "No Building")]
        public Building Building { get; set; }
        [DisplayFormat(NullDisplayText = "No Employees")]
        public ICollection<Employee> Employees { get; set; }

        public bool ContainsDepartment(int id)
        {
            if (Departments == null) return false;
            foreach (var department in Departments)
            {
                if (department.DepartmentId == id) return true;
            }
            return false;
        }
    }
}
