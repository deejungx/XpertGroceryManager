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
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        [Authorize]
        public async Task<IActionResult> Index(
            string sortOrder,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            var products = from p in _context.Products
                select p;

            products = sortOrder switch
            {
                "name_desc" => products.OrderByDescending(p => p.Name),
                _ => products.OrderBy(p => p.Name),
            };

            products = products.Include(p => p.Category);

            int pageSize = 12;
            return View(await PaginatedList<Product>.CreateAsync(products.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Products/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Stock)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,CategoryId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                            .Include(p => p.Category)
                            .Include(p => p.Stock)
                            .Include(p => p.PurchaseLineItems)
                                .ThenInclude(li => li.Purchase)
                            .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Product product = await _context.Products
                .Include(s => s.Stock)
                .SingleAsync(i => i.Id == id);

            var stock = await _context.Stocks
                .Where(s => s.Id == id)
                .ToListAsync();

            stock.ForEach(s => s.Quantity = 0);

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Product Stock
        [Authorize]
        public async Task<IActionResult> ProductStock(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["QuantitySortParam"] = sortOrder == "Quantity" ? "quantity_desc" : "Quantity";
            
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var products = from p in _context.Products
                           select p;

            products = products.Where(p => p.Stock != null && p.Stock.Quantity != 0);

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.Name.Contains(searchString)
                                       || p.Description.Contains(searchString));
            }

            products = sortOrder switch
            {
                "name_desc" => products.OrderByDescending(p => p.Name),
                "Quantity" => products.OrderBy(p => p.Stock.Quantity),
                "quantity_desc" => products.OrderByDescending(p => p.Stock.Quantity),
                _ => products.OrderBy(p => p.Name),
            };

            products = products.Include(p => p.Category)
                               .Include(p => p.Stock);

            int pageSize = 12;
            return View(await PaginatedList<Product>.CreateAsync(products.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Stock Clearance
        [Authorize]
        public async Task<IActionResult> StockClearance(
            string sortOrder,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            var now = DateTime.UtcNow;
            var threshold = now.AddDays(-180);

            var products = from p in _context.Products
                            select p;

            var purchases = from p in _context.Purchases
                            where p.PurchaseDate <= threshold
                            select p;

            products = products
                        .Where(p => p.PurchaseLineItems
                                        .Any(l => l.Purchase.PurchaseDate <= threshold))
                        .Where(p => p.Stock.Quantity != 0);

            products = sortOrder switch
            {
                "name_desc" => products.OrderByDescending(p => p.Name),
                _ => products.OrderBy(p => p.Name),
            };

            products = products
                        .Include(p => p.Category)
                        .Include(p => p.Stock)
                        .Include(p => p.PurchaseLineItems)
                            .ThenInclude(li => li.Purchase);

            int pageSize = 12;
            return View(await PaginatedList<Product>.CreateAsync(products.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // POST: Products/Delete/Clearance
        [Authorize]
        [HttpPost, ActionName("ClearStock")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StockClearanceConfirmed()
        {
            var now = DateTime.UtcNow;
            var threshold = now.AddDays(-180);

            var products = from p in _context.Products
                            select p;
            var stocks = from s in _context.Stocks
                            select s;

            products = products
                        .Where(p => p.PurchaseLineItems
                                        .Any(l => l.Purchase.PurchaseDate <= threshold));

            foreach (var product in products)
            {
                stocks.First(s => s.Id == product.Id).Quantity = 0;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(StockClearance));
        }

        // GET: Out of Stock Products
        [Authorize]
        public async Task<IActionResult> OutOfStock(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["QuantitySortParam"] = sortOrder == "Quantity" ? "quantity_desc" : "Quantity";
            ViewData["DateSortParam"] = sortOrder == "Date" ? "date_desc" : "Date";
            
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var products = from p in _context.Products
                           where p.PurchaseLineItems.Any()
                           select p;

            products = products.Where(p => p.Stock == null || p.Stock.Quantity <= 10);

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.Name.Contains(searchString)
                                       || p.Description.Contains(searchString));
            }

            products = sortOrder switch
            {
                "name_desc" => products.OrderByDescending(p => p.Name),
                "Quantity" => products.OrderBy(p => p.Stock.Quantity),
                "quantity_desc" => products.OrderByDescending(p => p.Stock.Quantity),
                "Date" => products.OrderBy(p => p.PurchaseLineItems.First().Purchase.PurchaseDate),
                "date_desc" => products.OrderByDescending(p => p.PurchaseLineItems.First().Purchase.PurchaseDate),
                _ => products.OrderBy(p => p.Name)
                             .OrderByDescending(p => p.Stock.Quantity)
                             .OrderByDescending(p => p.PurchaseLineItems.First().Purchase.PurchaseDate),
            };

            products = products
                        .Include(p => p.Category)
                        .Include(p => p.Stock)
                        .Include(p => p.PurchaseLineItems)
                            .ThenInclude(li => li.Purchase);

            int pageSize = 12;
            return View(await PaginatedList<Product>.CreateAsync(products.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Unsold Products
        [Authorize]
        public async Task<IActionResult> UnsoldProducts(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["QuantitySortParam"] = sortOrder == "Quantity" ? "quantity_desc" : "Quantity";
            var now = DateTime.UtcNow;
            var threshold = now.AddDays(-31);
            
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var products = from p in _context.Products
                           select p;

            products = products.Where(p => p.Stock != null && p.Stock.Quantity != 0);
            products = products
                        .Where(p => !p.SalesLineItems.Any() || p.SalesLineItems
                        .OrderByDescending(s => s.Sales.SalesDate)
                        .First().Sales.SalesDate <= threshold);

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.Name.Contains(searchString)
                                       || p.Description.Contains(searchString));
            }

            products = sortOrder switch
            {
                "name_desc" => products.OrderByDescending(p => p.Name),
                "Quantity" => products.OrderBy(p => p.Stock.Quantity),
                "quantity_desc" => products.OrderByDescending(p => p.Stock.Quantity),
                _ => products.OrderBy(p => p.Name),
            };

            products = products
                        .Include(p => p.Category)
                        .Include(p => p.Stock)
                        .Include(p => p.SalesLineItems)
                            .ThenInclude(li => li.Sales);

            int pageSize = 12;
            return View(await PaginatedList<Product>.CreateAsync(products.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
