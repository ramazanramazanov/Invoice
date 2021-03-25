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
    [Authorize(Roles ="Admin,User")]
    public class InvoiceController : Controller
    {
        private readonly ApplicationDbContext _context;
        public InvoiceController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Invoices()
        {
           var invoices= _context.Invoices.ToList();
            return View(invoices);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Invoices invoice)
        {
            _context.Invoices.Add(invoice);
            _context.SaveChanges();
            return RedirectToAction("Invoices", "Invoice", new { area = "Admin" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomer()
        {
            return Json(await _context.Customers.ToListAsync());

        }

        [HttpGet]
        public IActionResult Search(string? searchInvoice)
        {
            var selectedinvoice=new List<Invoices>();
            if (!string.IsNullOrEmpty(searchInvoice))
            {
                 selectedinvoice = _context.Invoices.Where(x => x.Projectname.ToLower().Contains(searchInvoice.ToLower())).ToList();
                if(selectedinvoice.Count==0)
                {
                    var customers = _context.Customers.ToList().Where(x => x.Name.ToLower().Contains(searchInvoice.ToLower())).ToList();
                    foreach (var customer in customers)
                    {
                      selectedinvoice=  _context.Invoices.ToList().Where(x => x.CustomerId == customer.Id).ToList();
                    }
                }
                selectedinvoice = selectedinvoice.Distinct().ToList();

            }
            return View(selectedinvoice);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Invoices>>> Order()
        {
            return await _context.Invoices.OrderByDescending(x => x.InvoiceDate).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<Invoices>>> DateFilter(DateTime? start, DateTime? end)
        {
            List<Invoices> invoices = _context.Invoices.ToList();

            if (start != null && end != null)
            {
                invoices = invoices.Where(x => x.InvoiceDate > start && x.InvoiceDate < end).OrderByDescending(x => x.InvoiceDate).ToList();

            }
            else if (start != null && end == null)
            {
                invoices = _context.Invoices.Where(x => x.InvoiceDate > start).OrderByDescending(x => x.InvoiceDate).ToList();
            }
            else if (end != null && start == null)
            {
                invoices = _context.Invoices.Where(x => x.InvoiceDate < end).OrderByDescending(x => x.InvoiceDate).ToList();
            }
            else
            {
                invoices = await _context.Invoices.ToListAsync();
            }

            return invoices;
        }
        [HttpPost]
        public IActionResult DeleteOrder(int deleteID)
        {
            var find = _context.Invoices.Find(deleteID);

            _context.Invoices.Remove(find);
            _context.SaveChanges();
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var invoices = _context.Invoices.Find(id);
            return View(invoices);
        }
        [HttpPost]
        public IActionResult Edit(int id, Invoices invoice)
        {
            var curreninvoice = _context.Invoices.Find(id);
            curreninvoice.InvoiceDate = invoice.InvoiceDate;
            curreninvoice.InvoiceNumber = invoice.InvoiceNumber;
            curreninvoice.Netamount = invoice.Netamount;
            curreninvoice.Paymentstatus = invoice.Paymentstatus;
            curreninvoice.Projectname = invoice.Projectname;
            curreninvoice.Taxamount = invoice.Taxamount;
            curreninvoice.Totalamount = invoice.Totalamount;
            curreninvoice.InvoiceDate = invoice.InvoiceDate;
            _context.SaveChanges();

            return RedirectToAction("Invoices", "Invoice", new { area = "Admin" });
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var curreninvoice = _context.Invoices.Find(id);
            if (curreninvoice == null)
            {
                ModelState.AddModelError("", "This invoice is not defined");
                return View();
            }
            return View(curreninvoice);
        }
        [HttpPost]
        public IActionResult Delete(int? id)
        {
            var curreninvoice = _context.Invoices.Find(id);
            if (curreninvoice == null)
            {
                ModelState.AddModelError("", "This invoice is not defined");
                return View();
            }
            _context.Invoices.Remove(curreninvoice);
            _context.SaveChanges();
            return RedirectToAction("Invoices", "Invoice", new { area = "Admin" });
        }

    }
}