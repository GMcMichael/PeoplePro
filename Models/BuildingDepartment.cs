namespace PeoplePro.Models
{
    public class BuildingDepartment
    {
        public int BuildingId { get; set; }
        public Building Building { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
