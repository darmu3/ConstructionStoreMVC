using LinqToDB;
using LinqToDB.AspNet;
using Microsoft.AspNetCore.Mvc;
using applicationmvc.Models;
using System.Threading.Tasks;
using LinqToDB.Data;

namespace applicationmvc.Controllers
{
    public class StoresController : Controller
    {
        private readonly DataConnection _db;

        public StoresController(DataConnection db)
        {
            _db = db;
        }

        // GET: Stores
        public async Task<IActionResult> Index()
        {
            var stores = await _db.GetTable<Store>().ToListAsync();
            return View(stores);
        }

        // GET: Stores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var store = await _db.GetTable<Store>().FirstOrDefaultAsync(m => m.StoreId == id);

            if (store == null)
            {
                return NotFound();
            }

            return View(store);
        }

        // GET: Stores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StoreId,StoreName,StoreAddress")] Store store)
        {
            ModelState.Remove("Orders");
            if (ModelState.IsValid)
            {
                await _db.InsertAsync(store);
                return RedirectToAction(nameof(Index));
            }

            return View(store);
        }

        // GET: Stores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var store = await _db.GetTable<Store>().FirstOrDefaultAsync(m => m.StoreId == id);

            if (store == null)
            {
                return NotFound();
            }

            return View(store);
        }

        // POST: Stores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StoreId,StoreName,StoreAddress")] Store store)
        {
            if (id != store.StoreId)
            {
                return NotFound();
            }

            ModelState.Remove("Orders");
            if (ModelState.IsValid)
            {
                try
                {
                    await _db.UpdateAsync(store);
                }
                catch (LinqToDBException)
                {
                    if (!StoreExists(store.StoreId))
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

            return View(store);
        }

        // GET: Stores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var store = await _db.GetTable<Store>().FirstOrDefaultAsync(m => m.StoreId == id);

            if (store == null)
            {
                return NotFound();
            }

            return View(store);
        }

        // POST: Stores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var store = await _db.GetTable<Store>().FirstOrDefaultAsync(m => m.StoreId == id);

            if (store != null)
            {
                await _db.DeleteAsync(store);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool StoreExists(int id)
        {
            return _db.GetTable<Store>().Any(e => e.StoreId == id);
        }
    }
}
