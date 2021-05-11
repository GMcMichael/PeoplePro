using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeoplePro.Models
{
    public class Department
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<DepartmentRoom> DepartmentRooms { get; set; }
    }
}
