using System.ComponentModel.DataAnnotations;

namespace PeoplePro.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int DepartmentId { get; set; }
        [DisplayFormat(NullDisplayText = "No Department")]
        public Department Department { get; set; }
        public int RoomId { get; set; }
        [DisplayFormat(NullDisplayText = "No Room")]
        public Room Room { get; set; }
    }
}
