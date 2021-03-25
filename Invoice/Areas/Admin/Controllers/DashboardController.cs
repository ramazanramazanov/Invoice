using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Invoice.Areas.Admin.Models;
using Invoice.Data;
using Invoice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Invoice.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        //private readonly SignInManager<AppUser> _signInManager;
        //private readonly IPasswordValidator<AppUser> _passwordValidator;
        private readonly IConfiguration _configuration;
        private readonly List<string> _roles;
        public DashboardController(UserManager<ApplicationUser> userManager,ApplicationDbContext context, 
            //SignInManager<AppUser> signInManager,
            //                             IPasswordValidator<AppUser> passwordValidator,
                                          IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            //_signInManager = signInManager;
            //_passwordValidator = passwordValidator;
            _configuration = configuration;
            _roles = new List<string>();
        }
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles ="Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> Users()
        {
            return  View(await _userManager.Users.ToListAsync());
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public  IActionResult Edit(int id)
        {
            ApplicationUser user = _userManager.Users.FirstOrDefault(x => x.Id == id);
            return View(user);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(int id,ApplicationUser appUser)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == id);
            user.UserName = appUser.UserName;
            user.AccessFailedCount = appUser.AccessFailedCount;
            user.EmailConfirmed = appUser.EmailConfirmed;
            user.Email = appUser.Email;
            user.LockoutEnabled = appUser.LockoutEnabled;
            user.LockoutEnd = appUser.LockoutEnd;
            user.NormalizedEmail = appUser.NormalizedEmail;
            user.NormalizedUserName = appUser.NormalizedUserName;
            user.PasswordHash = appUser.PasswordHash;
            user.PhoneNumber = appUser.PhoneNumber;
            user.PhoneNumberConfirmed = appUser.PhoneNumberConfirmed;
            user.SecurityStamp = appUser.SecurityStamp;
            user.ConcurrencyStamp = appUser.ConcurrencyStamp;
            user.TwoFactorEnabled = appUser.TwoFactorEnabled;
            _userManager.UpdateAsync(user).GetAwaiter().GetResult();
            _context.SaveChanges();

            return RedirectToAction("Users", "Dashboard", new { area = "Admin" });
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult<ApplicationUser> Delete(int id)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == id);
            return View(user);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Delete(int? id)
        {
             var user = _userManager.Users.FirstOrDefault(x => x.Id == id);
            _userManager.DeleteAsync(user).GetAwaiter().GetResult();
            _context.SaveChanges();
            return RedirectToAction("Users", "Dashboard", new { area = "Admin" });
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(RegisterModel model)
        {
            ApplicationUser currentUser = await _userManager.FindByEmailAsync(model.Email);
            if (currentUser == null)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Email
                };
                var identityResult = await _userManager.CreateAsync(user, model.Password);

                if (identityResult.Succeeded)
                {
                    var roles = CreateRoles();
                    await _userManager.AddToRoleAsync(user, roles[1]);
                    return View();
                }
                else
                {
                    foreach (var error in identityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    
                    ModelState.AddModelError("", "Emal is already exits");
                    return View();
                }

            }
            else
            {
                ModelState.AddModelError("", "Emal is already exits");
                return View();
            }
            
        }
        public List<string> CreateRoles()
        {
            var roles = _configuration.GetSection("Roles").Value.Split(",");
            foreach (var role in roles)
            {
                _roles.Add(role);
            }
            return _roles;
        }

    }
}