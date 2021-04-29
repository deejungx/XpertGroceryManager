using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using XpertGroceryManager.Data;
using XpertGroceryManager.Models;
using XpertGroceryManager.Utils;

namespace XpertGroceryManager.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Customers
        [Authorize]
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;

            var customers = from c in _context.Customers
                select c;

            if (!string.IsNullOrEmpty(searchString))
            {
                customers = customers.Where(c => c.Name.Contains(searchString)
                                       || c.MemberNumber.StartsWith(searchString));
            }

            customers = sortOrder switch
            {
                "name_desc" => customers.OrderByDescending(p => p.Name),
                _ => customers.OrderBy(p => p.Name),
            };

            customers = customers.Include(c => c.Sales);

            int pageSize = 12;
            return View(await PaginatedList<Customer>.CreateAsync(customers.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Customers/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var now = DateTime.UtcNow;
            var threshold = now.AddDays(-31);
            var customer = await _context.Customers
                .Include(c => c.Sales)
                    .ThenInclude(s => s.LineItems)
                        .ThenInclude(li => li.Product)
                .Select(c => new Customer()
                {
                    Id = c.Id,
                    Name = c.Name,
                    MemberNumber = c.MemberNumber,
                    MemberType = c.MemberType,
                    Phone = c.Phone,
                    Address = c.Address,
                    Email = c.Email,
                    Sales = (ICollection<Sales>)c.Sales.Where(s => s.SalesDate >= threshold)
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,MemberNumber,MemberType,Phone,Address,Email")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,MemberNumber,MemberType,Phone,Address,Email")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
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
            return View(customer);
        }

        // GET: Customers/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Inactive Customers
        [Authorize]
        public async Task<IActionResult> InactiveCustomers(
            string sortOrder,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            var now = DateTime.UtcNow;
            var threshold = now.AddDays(-31);

            pageNumber = 1;

            var customers = from c in _context.Customers
                            where c.Sales.Any()
                            select c;

            customers = customers.Where(c => c.Sales.OrderByDescending(s => s.SalesDate).First().SalesDate <= threshold);

            customers = sortOrder switch
            {
                "name_desc" => customers.OrderByDescending(p => p.Name),
                _ => customers.OrderBy(p => p.Name),
            };

            customers = customers.Include(c => c.Sales);

            int pageSize = 12;
            return View(await PaginatedList<Customer>.CreateAsync(customers.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
