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
        //public DbSet<DepartmentRoom> DepartmentRooms { get; set; }//Add if I want to query for DepartmentRooms directly instead of Rooms and Departments

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //TODO: Add Tables here
            modelBuilder.Entity<Room>().ToTable("Rooms");
            modelBuilder.Entity<Department>().ToTable("Department");

            //TODO: Add relationships here
            //TODO: see https://docs.microsoft.com/en-us/ef/core/modeling/relationships#many-to-many
            modelBuilder.Entity<DepartmentRoom>()
            .HasKey(dr => new { dr.DepartmentId, dr.RoomId });

            modelBuilder.Entity<DepartmentRoom>()
                .HasOne(dr => dr.Department)
                .WithMany(d => d.DepartmentRooms)
                .HasForeignKey(dr => dr.DepartmentId);

            modelBuilder.Entity<DepartmentRoom>()
                .HasOne(dr => dr.Room)
                .WithMany(r => r.DepartmentRooms)
                .HasForeignKey(dr => dr.RoomId);
        }
    }
}
