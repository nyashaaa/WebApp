using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LoginWebApp.Data;
using LoginWebApp.Models;

namespace LoginWebApp.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public EmployeeController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Employee
        public async Task<IActionResult> Index()
        {
            return View(await _context.Employees.ToListAsync());
        }

        // GET: Employee/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var employee = await _context.Employees.FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null) return NotFound();

            return View(employee);
        }

        // GET: Employee/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,Position,Department,Email,Phone,DateHired")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employee/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();

            return View(employee);
        }

        // POST: Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Position,Department,Email,Phone,DateHired")] Employee employee)
        {
            if (id != employee.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var employee = await _context.Employees.FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null) return NotFound();

            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // MyProfile - Display logged-in employee profile by email matching
        [HttpGet]
        public async Task<IActionResult> MyProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Email == user.Email);
            if (employee == null) return NotFound("Employee profile not found for your email.");

            return View(employee);
        }

        // Optional: Edit phone number only for logged-in employee
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Email == user.Email);
            if (employee == null) return NotFound();

            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile([Bind("Id,Phone")] Employee input)
        {
            var employee = await _context.Employees.FindAsync(input.Id);
            if (employee == null) return NotFound();

            employee.Phone = input.Phone;
            _context.Update(employee);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(MyProfile));
        }


        private bool EmployeeExists(int id) => _context.Employees.Any(e => e.Id == id);
    }
}
