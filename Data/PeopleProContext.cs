using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PeoplePro.Models;

namespace PeoplePro.Data
{
    public class PeopleProContext : DbContext
    {
        public PeopleProContext (DbContextOptions<PeopleProContext> options)
            : base(options)
        {
        }

        //TODO: Add Models here
        public DbSet<Room> Rooms { get; set; }
        public DbSet<DepartmentRoom> DepartmentsRooms { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<BuildingDepartment> BuildingsDepartments { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)//Only many to many need a seperate class eg. DepartmantsRooms
        {
            //TODO: Add Tables here
            modelBuilder.Entity<Room>().ToTable("Rooms");
            modelBuilder.Entity<Department>().ToTable("Departments");
            modelBuilder.Entity<Building>().ToTable("Buildings");
            modelBuilder.Entity<Employee>().ToTable("Employees");

            //TODO: Add relationships here

            //Department - Room relationships (many to many)
            modelBuilder.Entity<DepartmentRoom>()
            .HasKey(dr => new { dr.DepartmentId, dr.RoomId });

            //Department - Employee relationship (one to many)
            modelBuilder.Entity<Department>()
                .HasMany(d => d.Employees)
                .WithOne(e => e.Department)
                .OnDelete(DeleteBehavior.SetNull);

            //Room - Employee relationship (one to many)
            modelBuilder.Entity<Room>()
                .HasMany(r => r.Employees)
                .WithOne(e => e.Room)
                .OnDelete(DeleteBehavior.SetNull);

            //Building - Room relationship (one to many)
            modelBuilder.Entity<Building>()
                .HasMany(b => b.Rooms)
                .WithOne(r => r.Building)
                .OnDelete(DeleteBehavior.SetNull);

            //Building - Department relationship (many to many)
            modelBuilder.Entity<BuildingDepartment>()
                .HasKey(bd => new { bd.BuildingId, bd.DepartmentId });
        }
    }
}
