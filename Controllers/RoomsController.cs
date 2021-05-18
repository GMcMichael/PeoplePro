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
    public class RoomsController : Controller
    {
        private readonly PeopleProContext _context;

        public RoomsController(PeopleProContext context)
        {
            _context = context;
        }

        // GET: Rooms
        public async Task<IActionResult> Index()
        {
            var peopleProContext = _context.Rooms
                .Include(r => r.Building)
                .Include(r => r.Departments)
                .Include(r => r.Employees);
            return View(await peopleProContext.ToListAsync());
        }

        // GET: Rooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = _context.Rooms
                .Include(r => r.Building)
                .Include(r => r.Departments)
                .ThenInclude(d => d.Department)
                .Include(r => r.Employees)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(await room);
        }

        // GET: Rooms/Create
        public IActionResult Create()
        {
            PopulateSelectors();
            return View();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,BuildingID")] Room room, int[] selectedDepartments)
        {
            if (room == null) return NotFound();

            try
            {
                UpdateInformation(room, selectedDepartments);
                _context.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = room.Id });
            } catch(DbUpdateException /*e*/)
            {
                ModelState.AddModelError("", "Unable to save changes. Please try again.");
            }

            PopulateSelectors(room);
            return View(room);
        }

        // GET: Rooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .Include(r => r.Employees)
                .Include(r => r.Departments)
                .Include(r => r.Building)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (room == null)
            {
                return NotFound();
            }
            PopulateSelectors(room);
            return View(room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, int[] selectedDepartments)
        {
            if (id == null) return NotFound();

            var roomToUpdate = await _context.Rooms
                .Include(r => r.Departments)
                .FirstOrDefaultAsync(r => r.Id == id);

            if(await TryUpdateModelAsync<Room>(roomToUpdate, "", d => d.Name, d => d.BuildingID))
            {
                try
                {
                    UpdateInformation(roomToUpdate, selectedDepartments);
                    _context.SaveChanges();
                    return RedirectToAction("Details", new { id = id });
                } catch (DbUpdateException /*e*/)
                {
                    ModelState.AddModelError("", "Unable to save changes. Please try again.");
                }
            }

            PopulateSelectors(roomToUpdate);
            return View(roomToUpdate);
        }

        // GET: Rooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .Include(r => r.Building)
                .Include(r => r.Departments)
                .Include(r => r.Employees)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomExists(int id)
        {
            return _context.Rooms.Any(e => e.Id == id);
        }

        private void PopulateSelectors(Room room = null)
        {
            var departmentData = new List<AssignedData>();
            var departmentQuery = from d in _context.Departments
                                  orderby d.Name
                                  select d;
            var buildingQuery = from b in _context.Buildings
                                orderby b.Name
                                select b;
            foreach (var department in departmentQuery)
            {
                departmentData.Add(new AssignedData
                {
                    TypeId = department.Id,
                    Name = department.Name,
                    Assigned = (room != null ? room.ContainsDepartment(department.Id) : false)
                });
            }
            ViewBag.Departments = departmentData;
            object selectedBuilding = null;
            if (room != null) selectedBuilding = room.BuildingID;
            ViewBag.BuildingId = new SelectList(buildingQuery, "Id", "Name", selectedBuilding);
        }

        private void UpdateInformation(Room room, int[] selectedDepartments)
        {
            if (selectedDepartments == null)
            {
                room.Departments = new List<DepartmentRoom>();
                return;
            }
            if (room.Departments == null) room.Departments = new List<DepartmentRoom>();
            var departmentRooms = new List<DepartmentRoom>(room.Departments);
            var currDepartmentRooms = new HashSet<int>(room.Departments.Select(d => d.DepartmentId));
            foreach (var department in _context.Departments)
            {
                var newDepartmentRoom = new DepartmentRoom
                {
                    DepartmentId = department.Id,
                    Department = department,
                    RoomId = room.Id,
                    Room = room
                };
                if(selectedDepartments.Contains(department.Id))
                {
                    if (!currDepartmentRooms.Contains(department.Id))
                        room.Departments.Add(newDepartmentRoom);
                } else if(currDepartmentRooms.Contains(department.Id))
                {
                    DepartmentRoom removeDepartment = departmentRooms.Find(d => d.DepartmentId.Equals(department.Id));
                    room.Departments.Remove(removeDepartment);
                }
            }
        }
    }
}
