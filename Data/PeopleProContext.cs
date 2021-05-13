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
        public DbSet<Department> Departments { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Employee> Employees { get; set; }
        //public DbSet<DepartmentRoom> DepartmentsRooms { get; set; }//add if I want to query DepartmentsRooms Directly

        protected override void OnModelCreating(ModelBuilder modelBuilder)//Only many to many need a seperate class eg. DepartmantsRooms
        {
            //TODO: Add Tables here
            modelBuilder.Entity<Room>().ToTable("Rooms");
            modelBuilder.Entity<Department>().ToTable("Departments");
            modelBuilder.Entity<Building>().ToTable("Buildings");
            modelBuilder.Entity<Employee>().ToTable("Employees");
            modelBuilder.Entity<DepartmentRoom>().ToTable("DepartmentsRooms");
            modelBuilder.Entity<BuildingDepartment>().ToTable("BuildingsDepartments");

            //TODO: Add relationships here
            //TODO: see https://docs.microsoft.com/en-us/ef/core/modeling/relationships#many-to-many

            //Department - Room relationships (many to many)
            modelBuilder.Entity<DepartmentRoom>()
            .HasKey(dr => new { dr.DepartmentId, dr.RoomId });

            modelBuilder.Entity<DepartmentRoom>()
                .HasOne(dr => dr.Department)
                .WithMany(dr => dr.Rooms)
                .HasForeignKey(dr => dr.DepartmentId);

            modelBuilder.Entity<DepartmentRoom>()
                .HasOne(dr => dr.Room)
                .WithMany(dr => dr.Departments)
                .HasForeignKey(dr => dr.RoomId);

            //Department - Employee relationship (one to many)
            modelBuilder.Entity<Department>()
                .HasMany(d => d.Employees)
                .WithOne(e => e.Department);

            //Room - Employee relationship (one to many)
            modelBuilder.Entity<Room>()
                .HasMany(r => r.Employees)
                .WithOne(e => e.Room);

            //Building - Room relationship (one to many)
            modelBuilder.Entity<Building>()
                .HasMany(b => b.Rooms)
                .WithOne(r => r.Building);

            //Building - Department relationship (many to many)
            modelBuilder.Entity<BuildingDepartment>()
                .HasKey(bd => new { bd.BuildingId, bd.DepartmentId });

            modelBuilder.Entity<BuildingDepartment>()
                .HasOne(bd => bd.Building)
                .WithMany(bd => bd.Departments)
                .HasForeignKey(bd => bd.BuildingId);

            modelBuilder.Entity<BuildingDepartment>()
                .HasOne(bd => bd.Department)
                .WithMany(bd => bd.Buildings)
                .HasForeignKey(bd => bd.DepartmentId);
        }
    }
}
