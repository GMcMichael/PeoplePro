using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PeoplePro.Models
{
    public class Building
    {
        public int Id { get; set; }
        [Display(Name = "Building")]
        public string Name { get; set; }
        [DisplayFormat(NullDisplayText = "No Rooms")]
        public ICollection<Room> Rooms { get; set; }
        [DisplayFormat(NullDisplayText = "No Departments")]
        public ICollection<BuildingDepartment> Departments { get; set; }
    }
}
