using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using XpertGroceryManager.Data;
using XpertGroceryManager.Models;
using XpertGroceryManager.Utils;

namespace XpertGroceryManager.Controllers
{
    public class HomeController : Controller
    {
#pragma warning disable IDE0052 // Remove unread private members
        private readonly ILogger<HomeController> _logger;
#pragma warning restore IDE0052 // Remove unread private members
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // GET: Dashboard
        [Authorize]
        public async Task<IActionResult> Dashboard(
            string sortOrder,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["QuantitySortParam"] = sortOrder == "Quantity" ? "quantity_desc" : "Quantity";
            
            pageNumber = 1;

            var products = from p in _context.Products
                           select p;

            ViewData["ProductCount"] = products.Count();

            products = from p in _context.Products
                        where p.Stock.Quantity <= 10
                        select p;
            var customers = from c in _context.Customers
                            select c;

            ViewData["CustomerCount"] = products.Count();

            products = sortOrder switch
            {
                "name_desc" => products.OrderByDescending(p => p.Name),
                "Quantity" => products.OrderBy(p => p.Stock.Quantity),
                "quantity_desc" => products.OrderByDescending(p => p.Stock.Quantity),
                _ => products.OrderByDescending(p => p.Stock.Quantity),
            };

            products = products.Include(p => p.Category)
                               .Include(p => p.Stock);

            int pageSize = 12;
            return View(await PaginatedList<Product>.CreateAsync(products.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
