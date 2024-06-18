using LinqToDB;
using Microsoft.AspNetCore.Mvc;
using applicationmvc.Models;
using applicationmvc.Context;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace applicationmvc.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;

        public UserController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: User/Login
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        // POST: User/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _db.GetTable<User>()
                    .FirstOrDefaultAsync(u => u.UserName == model.UserName && u.Password == model.Password);

                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim("UserEmail", user.UserEmail),
                        new Claim("PhoneNumber", user.PhoneNumber),
                        new Claim(ClaimTypes.Role, user.RoleId.ToString())
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return RedirectToAction("Index", "Products");
                }
                else
                {
                    ModelState.AddModelError("", "Неправильное имя пользователя или пароль");
                }
            }

            return View(model);
        }

        // GET: User/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: User/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User model)
        {
            ModelState.Remove("Role");
            if (ModelState.IsValid)
            {
                var existingUser = await _db.GetTable<User>()
                    .FirstOrDefaultAsync(u => u.UserName == model.UserName);

                if (existingUser != null)
                {
                    ModelState.AddModelError("UserName", "Пользователь с таким именем уже существует");
                    return View(model);
                }

                existingUser = await _db.GetTable<User>()
                    .FirstOrDefaultAsync(u => u.PhoneNumber == model.PhoneNumber);

                if (existingUser != null)
                {
                    ModelState.AddModelError("PhoneNumber", "Пользователь с таким номером телефона уже зарегистрирован");
                    return View(model);
                }

                existingUser = await _db.GetTable<User>()
                    .FirstOrDefaultAsync(u => u.UserEmail == model.UserEmail);

                if (existingUser != null)
                {
                    ModelState.AddModelError("UserEmail", "Пользователь с таким email уже зарегистрирован");
                    return View(model);
                }

                var user = new User
                {
                    UserName = model.UserName,
                    PhoneNumber = model.PhoneNumber,
                    UserEmail = model.UserEmail,
                    Password = model.Password,
                    RoleId = 1
                };

                await _db.InsertAsync(user);

                return RedirectToAction("Login", "User");
            }

            return View(model);
        }

        // POST: User/Logout
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "User");
        }

        // GET: User/ChangePassword
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View(new ChangePasswordViewModel());
        }

        // POST: User/ChangePassword
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userName = User.Identity.Name;
                var user = await _db.GetTable<User>().FirstOrDefaultAsync(u => u.UserName == userName);

                if (user != null && user.Password == model.CurrentPassword)
                {
                    user.Password = model.NewPassword;
                    await _db.UpdateAsync(user);

                    return RedirectToAction("Index", "Products");
                }

                ModelState.AddModelError("", "Текущий пароль неверен.");
            }

            return View(model);
        }
    }
}
