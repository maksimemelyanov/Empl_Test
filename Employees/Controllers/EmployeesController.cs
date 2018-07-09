using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Employees.Data;
using Employees.Model;

namespace Employees.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly EmployeesContext _context;

        public EmployeesController(EmployeesContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            var employeesContext = _context.Employees.Include(e => e.Chief);
            return View(await employeesContext.ToListAsync());
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Chief)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            ViewData["ChiefId"] = new SelectList(_context.Employees, "Id", "FullName");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,ChiefId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                var chief = await _context.Employees.Include(m=>m.Subordinates).SingleOrDefaultAsync(x => x.Id == employee.ChiefId);
                if (chief != null)
                    chief.Subordinates.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ChiefId"] = new SelectList(_context.Employees, "Id", "FullName", employee.ChiefId);
            return View(employee);
        }



        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Chief)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var employee = await _context.Employees.Include(x=>x.Subordinates).Include(x=>x.Chief).ThenInclude(x => x.Subordinates).SingleOrDefaultAsync(m => m.Id == id);
            if (employee != null)
            {
                if (employee.Chief != null)
                {
                    employee.Chief.Subordinates.Remove(employee);
                    foreach (var s in employee.Subordinates)
                    {
                        s.ChiefId = employee.ChiefId;
                        employee.Chief.Subordinates.Add(s);
                    }
                    await _context.SaveChangesAsync();
                }
                _context.Employees.Remove(employee);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(long id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }

    }
}
