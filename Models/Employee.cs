using System;
using System.ComponentModel.DataAnnotations;

namespace PeoplePro.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50)]
        public string Name { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        [DisplayFormat(NullDisplayText = "No Department")]
        public Department Department { get; set; }
        public Nullable<int> RoomId { get; set; }
        [DisplayFormat(NullDisplayText = "No Room")]
        public Room Room { get; set; }
    }
}
