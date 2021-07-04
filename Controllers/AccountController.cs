using System;
using System.Linq;
using System.Threading.Tasks;
using ForumApp.Models;
using ForumApp.Services;
using ForumApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace ForumApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View();
        }

         [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    UserName = model.Name
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Posts");
                }
                
                else
                {
                    foreach (var error in result.Errors)
                    {
                        if (error.Description.Contains("taken"))
                        {
                            ModelState.AddModelError("name", "Имя занято");
                        }
                        else if(error.Description.Contains("letters"))
                        {
                            ModelState.AddModelError("name", "Укажите ваше имя на латинице");    
                        }
                        else
                        {
                            ModelState.AddModelError(String.Empty, error.Description);
                        }
                    }
                }
            }
            return View(model);
        }

        
        public IActionResult Login()
        {
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByNameAsync(model.Name);
                if (user == null)
                    return NotFound("Пользователь не найден");
                
                var result = await _signInManager.PasswordSignInAsync(
                    user,
                    model.Password, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Posts");
                }
                else
                {
                    ModelState.AddModelError("", "Неверный логин или пароль");
                }
            }
            return View(model);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        
    }
}