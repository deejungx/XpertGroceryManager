using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using XpertGroceryManager.Data;
using XpertGroceryManager.Models;

namespace XpertGroceryManager.Controllers
{
    public class ApplicationRolesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApplicationRolesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ApplicationRoles
        public async Task<IActionResult> Index()
        {
            return View(await _context.Roles.ToListAsync());
        }

        // GET: ApplicationRoles/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationRole = await _context.Roles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationRole == null)
            {
                return NotFound();
            }

            return View(applicationRole);
        }

        // GET: ApplicationRoles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ApplicationRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,NormalizedName,ConcurrencyStamp")] ApplicationRole applicationRole)
        {
            if (ModelState.IsValid)
            {
                _context.Add(applicationRole);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(applicationRole);
        }

        // GET: ApplicationRoles/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationRole = await _context.Roles.FindAsync(id);
            if (applicationRole == null)
            {
                return NotFound();
            }
            return View(applicationRole);
        }

        // POST: ApplicationRoles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,NormalizedName,ConcurrencyStamp")] ApplicationRole applicationRole)
        {
            if (id != applicationRole.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(applicationRole);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationRoleExists(applicationRole.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(applicationRole);
        }

        // GET: ApplicationRoles/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationRole = await _context.Roles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationRole == null)
            {
                return NotFound();
            }

            return View(applicationRole);
        }

        // POST: ApplicationRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var applicationRole = await _context.Roles.FindAsync(id);
            _context.Roles.Remove(applicationRole);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationRoleExists(string id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }
    }
}
