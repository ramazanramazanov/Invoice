using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Invoice.Data;
using Invoice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Invoice.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles="Admin")]
    public class UserSideController : Controller
    {
        private readonly ApplicationDbContext _context;
        public UserSideController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Customer()
        {
            var customers= await  _context.Customers.ToListAsync();
            return View(customers);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            return  RedirectToAction("Customer", "UserSide", new { area = "Admin" });
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var customer = _context.Customers.Find(id);
            return View(customer);
        }
        [HttpPost]
        public IActionResult Edit(int id, Customer customer)
        {
            var currentcustomer = _context.Customers.Find(id);
            currentcustomer.Name = customer.Name;
            currentcustomer.Surname = customer.Surname;
            _context.SaveChanges();

            return RedirectToAction("Customer", "UserSide", new { area = "Admin" });
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var currentcustomer = _context.Customers.Find(id);
            if (currentcustomer == null)
            {
                ModelState.AddModelError("", "This customer is not defined");
                return View();
            }
            return View(currentcustomer);
        }
        [HttpPost]
        public IActionResult Delete(int? id)
        {
            var currentcustomer = _context.Customers.Find(id);
            if (currentcustomer == null)
            {
                ModelState.AddModelError("", "This customer is not defined");
                return View();
            }
            _context.Customers.Remove(currentcustomer);
            _context.SaveChanges();
            return RedirectToAction("Customer", "UserSide", new { area = "Admin" });
        }
    }
}