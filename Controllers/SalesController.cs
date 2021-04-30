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
using XpertGroceryManager.Models.ViewModels;

namespace XpertGroceryManager.Controllers
{
    public class SalesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SalesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Sales
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var sales = await _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.LineItems)
                    .ThenInclude(li => li.Product)
                .ToListAsync();
            var salesViewModelList = new List<SalesViewModel>();
            foreach (Sales s in sales)
            {
                var thisViewModel = new SalesViewModel
                {
                    SalesId = s.Id,
                    Date = s.SalesDate,
                    CustomerId = s.Customer.Id,
                    CustomerName = s.Customer.Name,
                    LineItems = GetLineItems(s)
                };
                salesViewModelList.Add(thisViewModel);
            }
            return View(salesViewModelList);
        }

        private List<LineItemViewModel> GetLineItems(Sales sales)
        {
            var lineItemViewModel = new List<LineItemViewModel>();
            foreach (var lineitem in sales.LineItems)
            {
                var li = new LineItemViewModel
                {
                    ProductId = lineitem.ProductId,
                    ProductName = lineitem.Product.Name,
                    Quantity = lineitem.Quantity,
                    UnitCost = lineitem.Product.Price
                };
                lineItemViewModel.Add(li);
            }
            return lineItemViewModel;
        }

        // GET: Sales/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sales = await _context.Sales
                .Include(s => s.Customer)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sales == null)
            {
                return NotFound();
            }

            return View(sales);
        }

        // GET: Sales/Create
        [Authorize]
        public IActionResult Create()
        {
            var salesViewModel = new SalesViewModel();
            var customers = _context.Customers
                                    .Select(c => new SelectListItem
                                    {
                                        Value = c.Id.ToString(),
                                        Text = c.Name
                                    });
            var products = _context.Products
                                    .Select(p => new SelectListItem
                                    {
                                        Value = p.Id.ToString(),
                                        Text = p.Name
                                    });
            ViewData["customers"] = customers.ToList();
            ViewData["products"] = products.ToList();
            return View(salesViewModel);
        }

        // POST: Sales/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SalesDate,CustomerId")] Sales sales,  string[] selectedProducts, string[] productQuantities)
        {
            if (selectedProducts != null)
            {
                sales.LineItems = new List<SalesLineItem>();
                var selectedProductsHS = new List<string>(selectedProducts);
                var productQuantitiesHS = new List<string>(productQuantities);

                foreach (var product in selectedProductsHS)
                {
                    int lineItemIndex = selectedProductsHS.FindIndex(p => p == product);
                    int quantity = int.Parse(productQuantitiesHS.ElementAt(lineItemIndex));
                    var lineItemToAdd = new SalesLineItem { SalesId=sales.Id, ProductId=int.Parse(product), Quantity=quantity };
                    sales.LineItems.Add(lineItemToAdd);
                }
            }
            if (ModelState.IsValid)
            {
                _context.Add(sales);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateCustomersDropDownList(sales.CustomerId);
            PopulateProductsData(sales);
            return View(sales);
        }

        // GET: Sales/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sales = await _context.Sales
                .Include(s => s.LineItems).ThenInclude(l => l.Product)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sales == null)
            {
                return NotFound();
            }
            PopulateCustomersDropDownList(sales.CustomerId);
            PopulateProductsData(sales);
            return View(sales);
        }

        // POST: Sales/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSales(int? id, string[] selectedProducts, string[] productQuantities)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesToUpdate = await _context.Sales
                .Include(s => s.LineItems)
                    .ThenInclude(l => l.Product)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (await TryUpdateModelAsync(
                salesToUpdate,
                "",
                s => s.SalesDate, s => s.CustomerId ))
            {
                UpdateSalesLineItems(selectedProducts, productQuantities, salesToUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
                return RedirectToAction(nameof(Index));
            }
            UpdateSalesLineItems(selectedProducts, productQuantities, salesToUpdate);
            PopulateCustomersDropDownList(salesToUpdate.CustomerId);
            PopulateProductsData(salesToUpdate);
            return View(salesToUpdate);
        }

        // GET: Sales/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sales = await _context.Sales
                .Include(s => s.Customer)
                .Include(i => i.LineItems)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sales == null)
            {
                return NotFound();
            }

            return View(sales);
        }

        // POST: Sales/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Sales sales = await _context.Sales
                .Include(s => s.Customer)
                .Include(i => i.LineItems)
                .SingleAsync(i => i.Id == id);

            var lineitems = await _context.SalesLineItems
                .Where(li => li.SalesId == id)
                .ToListAsync();
            lineitems.ForEach(li => li.SalesId = null);

            _context.Sales.Remove(sales);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalesExists(int id)
        {
            return _context.Sales.Any(e => e.Id == id);
        }

        private void PopulateCustomersDropDownList(object selectedCustomer = null)
        {
            var customersQuery = from c in _context.Customers
                                   orderby c.Name
                                   select c;
            ViewBag.CustomerId = new SelectList(customersQuery.AsNoTracking(), "Id", "Name", selectedCustomer);
        }

        private void PopulateProductsData(Sales sales)
        {
            var allProducts = _context.Products;
            var salesProducts = new HashSet<int>(sales.LineItems.Select(li => li.ProductId));
            var viewModel = new List<LineItemProductData>();
            foreach (var product in allProducts)
            {
                int quantity;
                if (salesProducts.Contains(product.Id))
                {
                    quantity = sales.LineItems.First(li => li.ProductId == product.Id).Quantity;
                }
                else
                {
                    quantity = 0;
                }
                viewModel.Add(new LineItemProductData
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Selected = salesProducts.Contains(product.Id),
                    Quantity = quantity
                });
            }
            ViewData["Products"] = viewModel;
        }

        private void UpdateSalesLineItems(string[] selectedProducts, string[] productQuantities, Sales salesToUpdate)
        {
            if (selectedProducts == null)
            {
                salesToUpdate.LineItems = new List<SalesLineItem>();
                return;
            }

            var selectedProductsHS = new List<string>(selectedProducts);
            var productQuantitiesHS = new List<string>(productQuantities);
            var lineItemProducts = new List<int>
                (salesToUpdate.LineItems.Select(li => li.Product.Id));
            foreach (var product in _context.Products)
            {
                if (selectedProductsHS.Contains(product.Id.ToString()))
                {
                    if (!lineItemProducts.Contains(product.Id))
                    {
                        int lineItemIndex = selectedProductsHS.FindIndex(p => p == product.Id.ToString());
                        int quantity = int.Parse(productQuantitiesHS.ElementAt(lineItemIndex));
                        salesToUpdate.LineItems.Add(new SalesLineItem { SalesId=salesToUpdate.Id, ProductId=product.Id, Quantity=quantity });
                    }
                }
                else
                {

                    if (lineItemProducts.Contains(product.Id))
                    {
                        SalesLineItem lineItemToRemove = salesToUpdate.LineItems.FirstOrDefault(i => i.ProductId == product.Id);
                        _context.Remove(lineItemToRemove);
                    }
                }
            }
        }
    }
}
