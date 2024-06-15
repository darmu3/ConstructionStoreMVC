using LinqToDB;
using LinqToDB.AspNet;
using Microsoft.AspNetCore.Mvc;
using applicationmvc.Models;
using System.Threading.Tasks;
using LinqToDB.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace applicationmvc.Controllers
{
    public class UserController : Controller
    {
        private readonly DataConnection _db;

        public UserController(DataConnection db)
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

                    Console.WriteLine("Успешный вход");
                    return RedirectToAction("Index", "Products");
                }
                else
                {
                    Console.WriteLine("Неуспешный вход");
                    ModelState.AddModelError("", "Неправильное имя пользователя или пароль");
                }
            }

            Console.WriteLine("Возвращаем представление с ошибкой");
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
                // Проверяем, существует ли пользователь с таким же именем пользователя
                var existingUser = await _db.GetTable<User>()
                    .FirstOrDefaultAsync(u => u.UserName == model.UserName);

                if (existingUser != null)
                {
                    ModelState.AddModelError("UserName", "Пользователь с таким именем уже существует");
                    return View(model);
                }

                // Проверяем, существует ли пользователь с таким же номером телефона
                existingUser = await _db.GetTable<User>()
                    .FirstOrDefaultAsync(u => u.PhoneNumber == model.PhoneNumber);

                if (existingUser != null)
                {
                    ModelState.AddModelError("PhoneNumber", "Пользователь с таким номером телефона уже зарегистрирован");
                    return View(model);
                }

                // Проверяем, существует ли пользователь с таким же email
                existingUser = await _db.GetTable<User>()
                    .FirstOrDefaultAsync(u => u.UserEmail == model.UserEmail);

                if (existingUser != null)
                {
                    ModelState.AddModelError("UserEmail", "Пользователь с таким email уже зарегистрирован");
                    return View(model);
                }

                // Создаем нового пользователя
                var user = new User
                {
                    UserName = model.UserName,
                    PhoneNumber = model.PhoneNumber,
                    UserEmail = model.UserEmail,
                    Password = model.Password,
                    RoleId = 1 // Назначаем роль "пользователь" с RoleId = 1
                };

                // Вставляем пользователя в базу данных
                await _db.InsertAsync(user);

                // Перенаправляем пользователя на страницу входа после успешной регистрации
                return RedirectToAction("Login", "User");
            }

            // Если модель не валидна, возвращаем пользователю представление с ошибками валидации
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
