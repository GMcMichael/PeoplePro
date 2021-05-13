using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PeoplePro.Models
{
    public class Building
    {
        public int Id { get; set; }
        [Display(Name = "Building")]
        public string Name { get; set; }

        public List<Room> Rooms { get; set; }
        public List<BuildingDepartment> Departments { get; set; }
    }
}
