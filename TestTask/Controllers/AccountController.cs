using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestTask.Models;
using TestTask.Models.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;



namespace TestTask.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        private IUserRepository repository;

        public AccountController(IUserRepository repository)
        {
            this.repository = repository;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()=> View();

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await repository.Users.FirstOrDefaultAsync(u => u.Login == model.Login && u.Password == model.Password);
                if (user != null)
                {
                    await Authenticate(user); // authorization

                    return RedirectToAction("List", "Error");
                }
                ModelState.AddModelError("", "User with this data was not found");
            }
            return View(model);
        }

        public IActionResult Settings() => View(repository.Users.FirstOrDefault(u=>u.Login == User.Identity.Name));

        [HttpPost]
        public IActionResult Settings(User user)
        {
            if (ModelState.IsValid)
            {
                repository.SaveUser(user);
                return RedirectToAction("Settings");
            }
            return RedirectToAction("Settings");  

        }

        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Login),
        };
            ClaimsIdentity id = new ClaimsIdentity(claims, "login");
            ClaimsPrincipal principal = new ClaimsPrincipal(id);
            await HttpContext.SignInAsync("Auth", principal);
        }

        [HttpGet]
        public IActionResult Register() => View();


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await repository.Users.FirstOrDefaultAsync(u => u.Login == model.Login);
                if (user == null)
                {
                    User newUser = new User { Login = model.Login, Password = model.Password };
                    repository.SaveUser(newUser);

                    return RedirectToAction("List", "Error");
                }
                else
                    ModelState.AddModelError("", "User already exists");
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                    scheme: "Auth");

            return RedirectToAction("Login");
        }

        public IActionResult List() => View(repository.Users);


    }
}
