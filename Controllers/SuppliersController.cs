using LinqToDB;
using Microsoft.AspNetCore.Mvc;
using applicationmvc.Models;
using applicationmvc.Context;

namespace applicationmvc.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly ApplicationDbContext _db;

        public SuppliersController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: Suppliers
        public async Task<IActionResult> Index()
        {
            var suppliers = await _db.GetTable<Supplier>().ToListAsync();
            return View(suppliers);
        }

        // GET: Suppliers/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _db.GetTable<Supplier>().FirstOrDefaultAsync(m => m.SupplierId == id);

            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        // GET: Suppliers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Suppliers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SupplierId,SupplierName,SupplierAddress")] Supplier supplier)
        {
            ModelState.Remove("Products");
            if (ModelState.IsValid)
            {
                await _db.InsertAsync(supplier);
                return RedirectToAction(nameof(Index));
            }

            return View(supplier);
        }

        // GET: Suppliers/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _db.GetTable<Supplier>().FirstOrDefaultAsync(m => m.SupplierId == id);

            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        // POST: Suppliers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SupplierId,SupplierName,SupplierAddress")] Supplier supplier)
        {
            if (id != supplier.SupplierId)
            {
                return NotFound();
            }

            ModelState.Remove("Products");
            if (ModelState.IsValid)
            {
                try
                {
                    await _db.UpdateAsync(supplier);
                }
                catch (LinqToDBException)
                {
                    if (!SupplierExists(supplier.SupplierId))
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

            return View(supplier);
        }

        // GET: Suppliers/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _db.GetTable<Supplier>().FirstOrDefaultAsync(m => m.SupplierId == id);

            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        // POST: Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var supplier = await _db.GetTable<Supplier>().FirstOrDefaultAsync(m => m.SupplierId == id);

            if (supplier != null)
            {
                await _db.DeleteAsync(supplier);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool SupplierExists(int id)
        {
            return _db.GetTable<Supplier>().Any(e => e.SupplierId == id);
        }
    }
}
