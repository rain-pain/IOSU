using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IOSU.Data;
using IOSU.Models;

namespace IOSU.Controllers
{
    public class ContractsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContractsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Contracts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Contract.Include(c => c.Client).Include(c => c.Manager).Include(c => c.Product);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Contracts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contract
                .Include(c => c.Client)
                .Include(c => c.Manager)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }

        // GET: Contracts/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Client, "Id", "Name");
            ViewData["ManagerPassportNumber"] = new SelectList(_context.Manager, "PassportNumber", "FullName");
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name");
            ViewBag.Products = _context.Product.ToList();
            return View();
        }

        // POST: Contracts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ManagerPassportNumber,ClientId,ProductId,AmountOfProduct,Date")] Contract contract)
        {
            if (ModelState.IsValid)
            {

                var product = await _context.Product.FirstOrDefaultAsync(x =>
                x.Id == contract.ProductId);
                if (product.Amount >= contract.AmountOfProduct)
                {
                    _context.Add(contract);
                    product.Amount -= contract.AmountOfProduct;
                    _context.Update(product);
                }
                else return RedirectToAction(nameof(Create));

                var client = await _context.Client.Include(x => x.Contracts).FirstOrDefaultAsync(x =>
                x.Id == contract.ClientId);
                if (client.Contracts.Count == 5)
                {
                    client.Discount = true;
                    _context.Update(client);
                }

                if (contract.AmountOfProduct >= 1000)
                {
                    var manager = await _context.Manager.FirstOrDefaultAsync(x =>
                    x.PassportNumber == contract.ManagerPassportNumber);
                    manager.Wage = manager.Wage * 102 / 100;
                    if (manager.Wage > 5000)
                    {
                        manager.Wage = 5000;
                    }
                    _context.Update(manager);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Client, "Id", "Name", contract.ClientId);
            ViewData["ManagerPassportNumber"] = new SelectList(_context.Manager, "PassportNumber", "FullName", contract.ManagerPassportNumber);
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name", contract.ProductId);
            return View(contract);
        }

        // GET: Contracts1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contract.FindAsync(id);
            if (contract == null)
            {
                return NotFound();
            }
            ViewBag.ClientId = new SelectList(_context.Client, "Id", "Name", contract.ClientId);
            ViewBag.ManagerPassportNumber = new SelectList(_context.Manager, "PassportNumber", "FullName", contract.ManagerPassportNumber);
            ViewBag.ProductId = new SelectList(_context.Product, "Id", "Name", contract.ProductId);
            return View(contract);
        }

        // POST: Contracts1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ManagerPassportNumber,ClientId,ProductId,AmountOfProduct,Date")] Contract contract)
        {
            if (id != contract.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contract);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContractExists(contract.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Client, "Id", "Id", contract.ClientId);
            ViewData["ManagerPassportNumber"] = new SelectList(_context.Manager, "PassportNumber", "PassportNumber", contract.ManagerPassportNumber);
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Id", contract.ProductId);
            return View(contract);
        }

        // GET: Contracts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contract.FindAsync(id);
            _context.Contract.Remove(contract);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContractExists(int id)
        {
            return _context.Contract.Any(e => e.Id == id);
        }
    }
}
