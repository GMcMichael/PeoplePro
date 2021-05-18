using PeoplePro.Models;
using System.Collections.Generic;
using System.Linq;

namespace PeoplePro.Data
{
    public static class DbInitializer
    {
        public static void Initialize(PeopleProContext context)
        {
            context.Database.EnsureCreated();

            //Room Seed
            if (!context.Rooms.Any())
            {
                var rooms = new Room[]
                {
                    new Room { Name="Room1" },
                    new Room { Name="Room2" },
                    new Room { Name="Room3" },
                    new Room { Name="Room4" }
                };
                context.Rooms.AddRange(rooms);
                context.SaveChanges();
            }

            //TODO: Seed the database with the other model data
            //Department Seed
            if (!context.Departments.Any())
            {
                var departments = new Department[]
                {
                    new Department { Name="Admissions" },
                    new Department { Name="Classroom Tech Services" },
                    new Department { Name="Alumni Relations" },
                    new Department { Name="Campus ID Center" }
                };
                context.Departments.AddRange(departments);
                context.SaveChanges();
            }
            //Buildings Seed
            if (!context.Buildings.Any())
            {
                var buildings = new Building[]
                {
                    new Building { Name="Learning Innovation Center" },
                    new Building { Name="Memorial Union" },
                    new Building { Name="Weatherford Hall" },
                    new Building { Name="Austin Hall" }
                };
                context.Buildings.AddRange(buildings);
                context.SaveChanges();
            }
            //Employee Seed
            if (!context.Employees.Any())
            {
                var departments = context.Departments.Select(d => d.Id).ToArray();
                var rooms = context.Rooms.Select(r => r.Id).ToArray();
                var employees = new Employee[]
                {
                    new Employee { Name="Jack", DepartmentId=departments[0], RoomId=rooms[0] },
                    new Employee { Name="Jill", DepartmentId=departments[1], RoomId=rooms[1] },
                    new Employee { Name="Gretta", DepartmentId=departments[2], RoomId=rooms[2] },
                    new Employee { Name="Bill", DepartmentId=departments[3], RoomId=rooms[3] }
                };
                context.Employees.AddRange(employees);
                context.SaveChanges();
            }
            //DepartmentRoom Seed
            if (!context.DepartmentsRooms.Any())
            {
                var departments = context.Departments.ToArray();
                var rooms = context.Rooms.ToArray();
                for (int i = 0; i < departments.Length; i++)
                {
                    departments[i].Rooms = new List<DepartmentRoom>
                    {
                        new DepartmentRoom
                        {
                            Department = departments[i],
                            Room = rooms[i]
                        }
                    };
                }
                context.SaveChanges();
            }
            //BuildingDepartment Seed
            if(!context.BuildingsDepartments.Any())
            {
                var buildings = context.Buildings.ToArray();
                var departments = context.Departments.ToArray();
                for (int i = 0; i < departments.Length; i++)
                {
                    departments[i].Buildings = new List<BuildingDepartment>
                    {
                        new BuildingDepartment
                        {
                            Department = departments[i],
                            Building = buildings[i]
                        }
                    };
                }
                context.SaveChanges();
            }
        }
    }
}
