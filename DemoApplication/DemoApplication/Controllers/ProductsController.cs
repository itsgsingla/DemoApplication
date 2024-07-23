using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DemoApplication.Data;
using DemoApplication.Models;
using Microsoft.AspNetCore.Authorization;
using DemoApplication.Repository;

namespace DemoApplication.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _context;

        public ProductsController(IProductRepository context)
        {
            _context = context;
        }

		[AllowAnonymous]
		public async Task<IActionResult> Index()
        {
            return View(await _context.ListAsync());
        }


		[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }


		[Authorize(Roles = "Admin")]
		public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Create([Bind("Id,Name,Price,Description")] Product product)
        {
            if (ModelState.IsValid)
            {
                await _context.CreateAsync(product);
                return RedirectToAction(nameof(Index));
            }
			else
				return View();
		}

		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

			var product = await _context.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Description")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
					await _context.UpdateAsync(product);
				}
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeleteConfirmed(int id)
        {
			var product = await _context.GetByIdAsync(id);
			if (product != null)
            {
                await _context.DeleteAsync(id);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            var product = _context.GetByIdAsync(id);
            return product != null;
        }
    }
}
