using LinqToDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using applicationmvc.Models;
using applicationmvc.Context;
using Microsoft.AspNetCore.Authorization;

namespace applicationmvc.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _db;

        public OrdersController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var currentUser = await _db.GetTable<User>().FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            bool isEmployeeOrAdmin = currentUser.RoleId == 2 || currentUser.RoleId == 3;

            List<Order> allOrders = new List<Order>();
            List<Order> myOrders = await _db.GetTable<Order>()
                                            .LoadWith(o => o.Store)
                                            .LoadWith(o => o.User)
                                            .Where(o => o.UserId == currentUser.UserId)
                                            .ToListAsync();

            if (isEmployeeOrAdmin)
            {
                allOrders = await _db.GetTable<Order>()
                                     .LoadWith(o => o.Store)
                                     .LoadWith(o => o.User)
                                     .ToListAsync();
            }

            var model = new OrdersViewModel
            {
                AllOrders = allOrders,
                MyOrders = myOrders,
                IsEmployeeOrAdmin = isEmployeeOrAdmin
            };

            return View(model);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _db.GetTable<Order>()
                                 .LoadWith(o => o.Store)
                                 .LoadWith(o => o.User) // Загружаем информацию о пользователе
                                 .FirstOrDefaultAsync(m => m.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            var productOrders = await _db.GetTable<ProductOrder>()
                                         .LoadWith(po => po.Product)
                                         .Where(po => po.OrderId == id)
                                         .ToListAsync();

            order.ProductOrders = productOrders;

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["StoreId"] = new SelectList(_db.GetTable<Store>(), "StoreId", "StoreName");
            ViewData["Products"] = _db.GetTable<Product>().ToList();
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order, int[] selectedProducts)
        {
            ModelState.Remove("Store");
            ModelState.Remove("User");
            ModelState.Remove("ProductOrders");
            ModelState.Remove("OrderNumber");

            // Получаем текущего пользователя
            var currentUser = await _db.GetTable<User>().FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            order.UserId = currentUser.UserId; // Устанавливаем UserId

            if (ModelState.IsValid)
            {
                try
                {
                    bool isFirstOrder = !_db.GetTable<Order>().Any();

                    if (isFirstOrder)
                    {
                        order.OrderNumber = "040-000001";
                    }
                    else
                    {
                        string lastOrderNumber = _db.GetTable<Order>()
                                                    .OrderByDescending(o => o.OrderId)
                                                    .Select(o => o.OrderNumber)
                                                    .FirstOrDefault();
                        int nextOrderNumber = int.Parse(lastOrderNumber.Substring(lastOrderNumber.LastIndexOf('-') + 1)) + 1;
                        order.OrderNumber = $"040-{nextOrderNumber.ToString("D6")}";
                    }

                    var orderId = await _db.InsertWithInt32IdentityAsync(order);

                    foreach (var productId in selectedProducts)
                    {
                        var productOrder = new ProductOrder { OrderId = orderId, ProductId = productId };
                        await _db.InsertAsync(productOrder);
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                    return View(order);
                }
            }

            ViewData["StoreId"] = new SelectList(_db.GetTable<Store>(), "StoreId", "StoreName", order.StoreId);
            ViewData["Products"] = _db.GetTable<Product>().ToList();
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _db.GetTable<Order>().FirstOrDefaultAsync(m => m.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            ViewData["StoreId"] = new SelectList(_db.GetTable<Store>(), "StoreId", "StoreName", order.StoreId);
            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,OrderNumber,OrderDate,OrderSum,StoreId,UserId")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            ModelState.Remove("Store");
            ModelState.Remove("ProductOrders");

            if (ModelState.IsValid)
            {
                try
                {
                    await _db.UpdateAsync(order);
                }
                catch (LinqToDBException)
                {
                    if (!OrderExists(order.OrderId))
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

            ViewData["StoreId"] = new SelectList(_db.GetTable<Store>(), "StoreId", "StoreName", order.StoreId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _db.GetTable<Order>()
                                 .LoadWith(o => o.Store)
                                 .LoadWith(o => o.User) // Загружаем информацию о пользователе
                                 .FirstOrDefaultAsync(m => m.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            var productOrders = await _db.GetTable<ProductOrder>()
                                         .LoadWith(po => po.Product)
                                         .Where(po => po.OrderId == id)
                                         .ToListAsync();

            order.ProductOrders = productOrders;

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _db.GetTable<Order>().FirstOrDefaultAsync(m => m.OrderId == id);

            if (order != null)
            {
                await _db.DeleteAsync(order);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _db.GetTable<Order>().Any(e => e.OrderId == id);
        }
    }
}
