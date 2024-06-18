using LinqToDB;
using Microsoft.AspNetCore.Mvc;
using applicationmvc.Models;
using applicationmvc.Context;

namespace applicationmvc.Controllers
{
    public class ProductCategoriesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductCategoriesController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: ProductCategories
        public async Task<IActionResult> Index()
        {
            var productCategories = await _db.GetTable<ProductCategory>().ToListAsync();
            return View(productCategories);
        }

        // GET: ProductCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCategory = await _db.GetTable<ProductCategory>().FirstOrDefaultAsync(m => m.ProductCategoryId == id);
            if (productCategory == null)
            {
                return NotFound();
            }

            return View(productCategory);
        }

        // GET: ProductCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductCategories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductCategoryId,CategoryName")] ProductCategory productCategory)
        {
            ModelState.Remove("Products");
            if (ModelState.IsValid)
            {
                try
                {
                    await _db.InsertAsync(productCategory);
                    return RedirectToAction(nameof(Index));
                }
                catch (LinqToDBException ex)
                {
                    Console.WriteLine("Error occurred while creating product category: " + ex.Message);
                    return View(productCategory);
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
                return View(productCategory);
            }
        }


        // GET: ProductCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCategory = await _db.GetTable<ProductCategory>().FirstOrDefaultAsync(m => m.ProductCategoryId == id);
            if (productCategory == null)
            {
                return NotFound();
            }
            return View(productCategory);
        }

        // POST: ProductCategories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductCategoryId,CategoryName")] ProductCategory productCategory)
        {
            if (id != productCategory.ProductCategoryId)
            {
                return NotFound();
            }

            ModelState.Remove("Products");
            if (ModelState.IsValid)
            {
                await _db.UpdateAsync(productCategory);
                return RedirectToAction(nameof(Index));
            }
            return View(productCategory);
        }

        // GET: ProductCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCategory = await _db.GetTable<ProductCategory>().FirstOrDefaultAsync(m => m.ProductCategoryId == id);
            if (productCategory == null)
            {
                return NotFound();
            }

            return View(productCategory);
        }

        // POST: ProductCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productCategory = await _db.GetTable<ProductCategory>().FirstOrDefaultAsync(m => m.ProductCategoryId == id);
            if (productCategory != null)
            {
                await _db.DeleteAsync(productCategory);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ProductCategoryExists(int id)
        {
            return _db.GetTable<ProductCategory>().Any(e => e.ProductCategoryId == id);
        }
    }
}
