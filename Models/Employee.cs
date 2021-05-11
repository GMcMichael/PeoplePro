using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeoplePro.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public string Name { get; set; }

        // TODO: Add some models https://docs.microsoft.com/en-us/ef/core/modeling/relationships#many-to-many
        //public List<Room> Rooms { get; set; }
    }
}
