using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PeoplePro.Data;
using PeoplePro.Models;

namespace PeoplePro.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly PeopleProContext _context;

        public DepartmentsController(PeopleProContext context)
        {
            _context = context;
        }

        // GET: Departments
        public async Task<IActionResult> Index()
        {
            var peopleProContext = _context.Departments
                .Include(d => d.Employees)
                .Include(d => d.Rooms)
                .Include(d => d.Buildings);
            return View(await peopleProContext.ToListAsync());
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = _context.Departments
                .Include(d => d.Employees)
                .Include(d => d.Rooms)
                .ThenInclude(r => r.Room)
                .Include(d => d.Buildings)
                .ThenInclude(b => b.Building)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(await department);
        }

        // GET: Departments/Create
        public IActionResult Create()
        {
            PopulateSelectors();
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Department department, int[] selectedRooms, int[]selectedBuildings)
        {
            if (department == null) return NotFound();

            try
            {
                UpdateInformation(department, selectedRooms, selectedBuildings);
                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = department.Id });
            }
            catch (DbUpdateException /*e*/)
            {
                ModelState.AddModelError("", "Unable to save changes. Please try again.");
            }
            PopulateSelectors(department);
            return View(department);
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(d => d.Employees)
                .Include(d => d.Rooms)
                .ThenInclude(r => r.Room)
                .Include(d => d.Buildings)
                .ThenInclude(b => b.Building)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (department == null)
            {
                return NotFound();
            }
            PopulateSelectors(department);
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, int[] selectedRooms, int[] selectedBuildings)
        {
            if (id == null) return NotFound();

            var departmentToUpdate = await _context.Departments
                .Include(d => d.Rooms)
                .FirstOrDefaultAsync(d => d.Id == id);

            if(await TryUpdateModelAsync<Department>(departmentToUpdate, "", d => d.Name))
            {
                try
                {
                    UpdateInformation(departmentToUpdate, selectedRooms, selectedBuildings);
                    _context.SaveChanges();
                    return RedirectToAction("Details", new { id = id });
                }
                catch (DbUpdateException /*e*/)
                {
                    ModelState.AddModelError("", "Unable to save changes. Please try again.");
                }
            }
            PopulateSelectors(departmentToUpdate);
            return View(departmentToUpdate);
        }

        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(d => d.Buildings)
                .Include(d => d.Rooms)
                .Include(d => d.Employees)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.Id == id);
        }

        private void PopulateSelectors(Department department = null)
        {
            var roomsData = new List<AssignedData>();
            var buildingsData = new List<AssignedData>();
            var roomQuery = from r in _context.Rooms
                            orderby r.Name
                            select r;
            var buildingQuery = from b in _context.Buildings
                                  orderby b.Name
                                  select b;
            foreach (var Room in roomQuery)
            {
                roomsData.Add(new AssignedData
                {
                    TypeId = Room.Id,
                    Name = Room.Name,
                    Assigned = (department != null ? department.ContainsRoom(Room.Id) : false)
                });
            }
            foreach (var building in buildingQuery)
            {
                buildingsData.Add(new AssignedData
                {
                    TypeId = building.Id,
                    Name = building.Name,
                    Assigned = (department != null ? department.ContainsBuilding(building.Id) : false)
                });
            }
            ViewBag.Rooms = roomsData;
            ViewBag.Buildings = buildingsData;
        }

        private void UpdateInformation(Department department, int[] selectedRooms, int[] selectedBuildings)
        {
            UpdateRooms(department, selectedRooms);
            UpdateBuildings(department, selectedBuildings);
        }

        private void UpdateRooms(Department department, int[] selectedRooms)
        {
            if (selectedRooms == null)
            {
                department.Rooms = new List<DepartmentRoom>();
                return;
            }
            if (department.Rooms == null) department.Rooms = new List<DepartmentRoom>();
            var departmentRooms = new List<DepartmentRoom>(department.Rooms);
            var currDepartmentRooms = new HashSet<int>(department.Rooms.Select(r => r.RoomId));
            foreach (var room in _context.Rooms)
            {
                var newDepartmentRoom = new DepartmentRoom
                {
                    DepartmentId = department.Id,
                    Department = department,
                    RoomId = room.Id,
                    Room = room
                };
                if (selectedRooms.Contains(room.Id))
                {
                    if (!currDepartmentRooms.Contains(room.Id))
                        department.Rooms.Add(newDepartmentRoom);
                } else if (currDepartmentRooms.Contains(room.Id))
                {
                    DepartmentRoom removeRoom = departmentRooms.Find(r => r.RoomId.Equals(room.Id));
                    department.Rooms.Remove(removeRoom);
                }
            }
        }

        private void UpdateBuildings(Department department, int[] selectedBuildings)
        {
            if (selectedBuildings == null)
            {
                department.Buildings = new List<BuildingDepartment>();
                return;
            }
            if (department.Buildings == null) department.Buildings = new List<BuildingDepartment>();
            var buildingDepartments = new List<BuildingDepartment>(department.Buildings);
            var currBuildingDepartments = new HashSet<int>(department.Buildings.Select(b => b.BuildingId));
            foreach (var building in _context.Buildings)
            {
                var newBuildingDepartment = new BuildingDepartment
                {
                    DepartmentId = department.Id,
                    Department = department,
                    BuildingId = building.Id,
                    Building = building
                };
                if (selectedBuildings.Contains(building.Id))
                {
                    if (!currBuildingDepartments.Contains(building.Id))
                        department.Buildings.Add(newBuildingDepartment);
                }
                else if (currBuildingDepartments.Contains(building.Id))
                {
                    BuildingDepartment removeBuilding = buildingDepartments.Find(b => b.BuildingId.Equals(building.Id));
                    department.Buildings.Remove(removeBuilding);
                }
            }
        }
    }
}
