using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackProject.DataAccessLayer;
using BackProject.DataAccessLayer.Entities;
using BackProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace BackProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]

        [ValidateAntiForgeryToken]

        [ActionName("Register")]
        

        public async Task<IActionResult>  RegisterName(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = new User
            {
                FullName = model.Fullname,
                Email = model.Email,
                UserName = model.UserName,
            };

            var existUsername = await _userManager.FindByNameAsync(model.UserName);

            if (existUsername != null)
            {
                ModelState.AddModelError("Username", "Bu adda Username mövcuddur");
                return View();
            }

           var roleResult= await _roleManager.CreateAsync(new IdentityRole
            {
                Name="Admin",

            });

            if (!roleResult.Succeeded)
            {
                foreach (var item in roleResult.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }

                return View();
            }

            var result =await _userManager.CreateAsync(user,model.Password);

            if (result.Succeeded)
            {
               await _userManager.AddToRoleAsync(user, "Admin");


                await _signInManager.SignInAsync(user, isPersistent: false);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }

                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]

        [ValidateAntiForgeryToken]

        [ActionName("Login")]

        public async Task<IActionResult> LoginName(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var existUser = await _userManager.FindByNameAsync(model.Username);

            if (existUser == null)
            {
                ModelState.AddModelError("", "Username or Password is incorrect");

                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(existUser, model.Password, false, true);

            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "You banned");

                return View();
            }

            if (!result.Succeeded)
            {
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}

