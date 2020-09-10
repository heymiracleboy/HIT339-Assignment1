﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ass1.Models;
using Microsoft.AspNetCore.Identity;
using Ass1.Areas.Identity.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace Ass1.Controllers
{
    public class CartsController : Controller
    {
        private readonly UserManager<Ass1User> _userManager;
        private readonly ItemDBContext _context;
        private readonly IHttpContextAccessor _session;

        public CartsController(ItemDBContext context, UserManager<Ass1User> userManager, IHttpContextAccessor session)
        {
            _userManager = userManager;
            _context = context;
            _session = session;
        }

        // GET: Carts
        public async Task<IActionResult> Index()
        {
   
            var cartId = _session.HttpContext.Session.GetString("cartId");
            var carts = _context.Carts
                .Where(c => c.CartId == cartId);

            return View(await carts.ToListAsync());
        }

        // GET: Carts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Carts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Item,Quantity,NameOfItem")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cart);
        }

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Item,Quantity,NameOfItem")] Cart cart)
        {
            if (id != cart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.Id))
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
            return View(cart);
        }

        // GET: Carts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            // add to cart
            var checkCount = _session.HttpContext.Session.GetInt32("cartCount");
            int cartCount = checkCount == null ? 0 : (int)checkCount;
            _session.HttpContext.Session.SetInt32("cartCount", --cartCount);

            return RedirectToAction(nameof(Index));
        }

        // GET: Items/Purchase
        [Authorize]
        public async Task<IActionResult> Purchase()
        {
            // get the cart id
            var cartId = _session.HttpContext.Session.GetString("cartId");

            // get the cart items
            var carts = _context.Carts
                .Where(c => c.CartId == cartId);

      

            // get the buyer
            var buyer = _userManager.GetUserName(User);       

            // create the sales
            foreach (Cart cart in carts.ToList())
            {
                // find the item
                var item = await _context.Item
                    .FirstOrDefaultAsync(m => m.ItemId == cart.Item);

                // update the quantity
                item.Quantity -= cart.Quantity;
                _context.Update(item);

                var totalSpent = cart.Quantity * item.Price;

                Sales sale = new Sales { Buyer = buyer, Item = cart.Item, Quantity = cart.Quantity , Name = item.Name, SellerPerson = item.Seller, TotalSpent = totalSpent};
                _context.Update(sale);
            }

            // Save the changes
            await _context.SaveChangesAsync();

            // delete cart
            _session.HttpContext.Session.SetString("cartId", "");
            _session.HttpContext.Session.SetInt32("cartCount", 0);

            return RedirectToAction(nameof(Index), "Items");
        }

        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.Id == id);
        }
    }
}
