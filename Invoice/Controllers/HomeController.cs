using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Invoice.Models;
using Microsoft.AspNetCore.Identity;
using SignInNS = Microsoft.AspNetCore.Identity;

namespace Invoice.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IPasswordValidator<ApplicationUser> _passwordValidator;

        public HomeController( UserManager<ApplicationUser> userManager, ILogger<HomeController> logger,
            SignInManager<ApplicationUser> signInManager,
                                         IPasswordValidator<ApplicationUser> passwordValidator)
        {
            _logger = logger;
            _signInManager = signInManager;
            _passwordValidator = passwordValidator;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginModel loginModel)
        {
            var currentuser = await _userManager.FindByEmailAsync(loginModel.Email);
            var checkPass = _passwordValidator.ValidateAsync(_userManager, currentuser, loginModel.Password);

            if (checkPass != null && checkPass.Result.Succeeded)
            {
                SignInNS.SignInResult signInResult = await _signInManager.PasswordSignInAsync(currentuser, loginModel.Password, true, false);

                if (signInResult.Succeeded)
                {

                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" }); ;
                }
                else
                {
                    ModelState.AddModelError("", "Emal is already exits");
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("", "Email or Password  incorrect");
                return View();
            }
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
