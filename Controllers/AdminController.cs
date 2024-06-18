using Microsoft.AspNetCore.Mvc;
using applicationmvc.Models;
using applicationmvc.Context;
using LinqToDB;

namespace applicationmvc.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Admin/ManageRoles
        public async Task<IActionResult> ManageRoles()
        {
            var model = new ManageRolesViewModel
            {
                Roles = await _context.Roles.ToListAsync()
            };

            return View(model);
        }

        // POST: /Admin/ManageRoles
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageRoles(string userName, int roleId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, $"Пользователь с именем \"{userName}\" не найден.");
                var model = new ManageRolesViewModel
                {
                    Roles = await _context.Roles.ToListAsync()
                };
                return View(model);
            }

            user.RoleId = roleId;

            await _context.UpdateAsync(user);

            return RedirectToAction(nameof(ManageRoles));
        }
    }
}
