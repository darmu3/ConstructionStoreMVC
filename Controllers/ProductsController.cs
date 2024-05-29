using LinqToDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using applicationmvc.Models;
using applicationmvc.Context;

namespace applicationmvc.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductsController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var products = _db.GetTable<Product>()
                              .LoadWith(p => p.ProductCategory)
                              .LoadWith(p => p.Supplier)
                              .ToList();
            return View(products);
        }


        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _db.GetTable<Product>()
                .LoadWith(p => p.ProductCategory)
                .LoadWith(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["ProductCategoryId"] = new SelectList(_db.GetTable<ProductCategory>(), "ProductCategoryId", "CategoryName");
            ViewData["SupplierId"] = new SelectList(_db.GetTable<Supplier>(), "SupplierId", "SupplierName");
            return View();
        }


        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Name,Description,Price,ProductCategoryId,SupplierId")] Product product)
        {
            ModelState.Remove("Supplier");
            ModelState.Remove("ProductCategory");
            ModelState.Remove("ProductOrders");
            ModelState.Remove("ProductWarehouses");

            if (ModelState.IsValid)
            {
                try
                {
                    await _db.InsertAsync(product);
                    return RedirectToAction(nameof(Index));
                }
                catch (LinqToDBException ex)
                {
                    Console.WriteLine("Error occurred while creating product: " + ex.Message);
                    return View(product);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("General error occurred while creating product: " + ex.Message);
                    return View(product);
                }
            }
            else
            {
                Console.WriteLine("Model state is invalid. Error count: " + ModelState.ErrorCount);
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine("Error: " + error.ErrorMessage);
                    }
                }
                ViewData["ProductCategoryId"] = new SelectList(_db.GetTable<ProductCategory>(), "ProductCategoryId", "CategoryName", product.ProductCategoryId);
                ViewData["SupplierId"] = new SelectList(_db.GetTable<Supplier>(), "SupplierId", "SupplierName", product.SupplierId);
                return View(product);
            }
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _db.GetTable<Product>().FirstOrDefaultAsync(m => m.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            ViewData["ProductCategoryId"] = new SelectList(_db.GetTable<ProductCategory>(), "ProductCategoryId", "CategoryName", product.ProductCategoryId);
            ViewData["SupplierId"] = new SelectList(_db.GetTable<Supplier>(), "SupplierId", "SupplierName", product.SupplierId);
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,Description,Price,ProductCategoryId,SupplierId")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            ModelState.Remove("Supplier");
            ModelState.Remove("ProductCategory");
            ModelState.Remove("ProductOrders");
            ModelState.Remove("ProductWarehouses");

            if (ModelState.IsValid)
            {
                try
                {
                    await _db.UpdateAsync(product);
                }
                catch (LinqToDBException)
                {
                    if (!ProductExists(product.ProductId))
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

            ViewData["ProductCategoryId"] = new SelectList(_db.GetTable<ProductCategory>(), "ProductCategoryId", "CategoryName", product.ProductCategoryId);
            ViewData["SupplierId"] = new SelectList(_db.GetTable<Supplier>(), "SupplierId", "SupplierName", product.SupplierId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _db.GetTable<Product>()
                .LoadWith(p => p.ProductCategory)
                .LoadWith(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _db.GetTable<Product>().FirstOrDefaultAsync(m => m.ProductId == id);

            if (product != null)
            {
                await _db.DeleteAsync(product);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _db.GetTable<Product>().Any(e => e.ProductId == id);
        }
    }
}
