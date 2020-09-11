using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ass1.Models;
using Ass1.Areas.Identity.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Ass1.Controllers
{
    
    public class ItemsController : Controller
    {
        private readonly UserManager<Ass1User> _userManager;
        private readonly ItemDBContext _context;
        private readonly IHttpContextAccessor _session;

        public ItemsController(ItemDBContext context, UserManager<Ass1User> userManager, IHttpContextAccessor session)
        {
            _userManager = userManager;
            _context = context;
            _session = session;
        }
        // GET: Items
        public async Task<IActionResult> Index(string searchString)
        {
            var filter = from m in _context.Item
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                filter = filter.Where(s => s.Name.Contains(searchString));
            }
            return View(await filter.ToListAsync());
        }

  

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Items/Create
        [Authorize (Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemId,Name,Description,Maker")] Item item)
        {
            if (ModelState.IsValid)
            {
                //Set variable seller (in the DB) as current logged on user
                var seller = _userManager.GetUserName(User);
                //Setting seller in item table as the current user 
                item.Seller = seller;
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        // GET: Items/Edit/5
 
        public async Task<IActionResult> Edit(int? id)
        {
            //Getting item context and current user, setting a variable seller to seller variable in db
            var items = await _context.Item.FindAsync(id);
            var user = _userManager.GetUserName(User);
            var seller = items.Seller;

            if (user != seller)
            {
                //error message to stop people from editing items not theirs
                ViewBag.errorMessage = "You have not listed this item, you cannot edit it";
                return View("Views/Home/Error.cshtml", ViewBag.errorMessage);
            }

            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ItemId,Name,Description,Maker,Seller")] Item item)
        {
            if (id != item.ItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.ItemId))
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
            return View(item);
        }

        // GET: Items/Delete/5

        public async Task<IActionResult> Delete(int? id)
        {

            //Getting item context and current user, setting a variable seller to seller variable in db
            var items = await _context.Item.FindAsync(id);
            var user = _userManager.GetUserName(User);
            var seller = items.Seller;

            if (user != seller)
            {
                //Error message to stop people from deleting items that are not theirs
                ViewBag.errorMessage = "You have not listed this item, you cannot delete it";
                return View("Views/Home/Error.cshtml", ViewBag.errorMessage);
            }

            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Item.FindAsync(id);
            _context.Item.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // GET: Items/Purchase/5
        public async Task<IActionResult> Purchase(int? id)
        {
            //Getting item context and current user, setting a variable seller to seller variable in db
            var items = await _context.Item
               .FirstOrDefaultAsync(m => m.ItemId == id);
            var user = _userManager.GetUserName(User);
            var seller = items.Seller;


            if (user == seller)
            {
                //Error message stopping people buying their own items
                ViewBag.errorMessage = "You cannot buy an item you listed";
                return View("Views/Home/Error.cshtml", ViewBag.errorMessage);
            }

            if (id == null)
            {
                return NotFound();
            }

           
            if (items == null)
            {
                return NotFound();
            }

            return View(items);
        }
        // POST: Items/Purchase/5
        [HttpPost, ActionName("Purchase")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PurchaseConfirmed([Bind("Item,Quantity,NameOfItem")] Cart cart, Sales sale)
        {
            // get or create a cart id
            string cartId = _session.HttpContext.Session.GetString("cartId");

            if (string.IsNullOrEmpty(cartId) == true) cartId = Guid.NewGuid().ToString();

            // use the cart id
            cart.CartId = cartId.ToString();

            //Getting context of the item table in db
            var item = await _context.Item
           .FirstOrDefaultAsync(m => m.ItemId == sale.Item);


            if (item == null)
            {
                return NotFound();
            }

            if (item.Quantity < sale.Quantity)
            {
                //Stock control, stopping people from adding more into their cart than is available
                ViewBag.errorMessage = "We do not have enough stock for your order.";
                return View("Views/Home/Error.cshtml", ViewBag.errorMessage);
            }

            if (item.Quantity == 0)
            {

                //Stock control, people cannot add an item with 0 stock
                ViewBag.errorMessage = "Sorry, we are out of stock.";
                return View("Views/Home/Error.cshtml", ViewBag.errorMessage);
            }

            //Getting name of item and seller so that the Nameofitem and seller row in carts table can be assigned
            var name = item.Name;
            cart.NameOfItem = name;

            var seller = item.Seller;
            cart.Seller = seller;
            var pricing = item.Price * cart.Quantity;
            cart.Price = pricing;

            // make the sale
            _context.Add(cart);

            // Save the changes
            await _context.SaveChangesAsync();

            // add to cart
            var checkCount = _session.HttpContext.Session.GetInt32("cartCount");
            int cartCount = checkCount == null ? 0 : (int)checkCount;
            _session.HttpContext.Session.SetString("cartId", cartId.ToString());
            _session.HttpContext.Session.SetInt32("cartCount", ++cartCount);

            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
            return _context.Item.Any(e => e.ItemId == id);
        }
    }
}
