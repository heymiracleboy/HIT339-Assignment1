using System;
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
using Ass1.ViewModel;

namespace Ass1.Controllers
{

    [Authorize]

   // [Route("UserPage/[Action]")]
    public class MyItemsController : Controller
    {

        private readonly UserManager<Ass1User> _userManager;
        private readonly ItemDBContext _context;
        private readonly IHttpContextAccessor _session;

        public MyItemsController(ItemDBContext context, UserManager<Ass1User> userManager, IHttpContextAccessor session)
        {
            _userManager = userManager;
            _context = context;
            _session = session;
        }
        //GET: MyItems/UserPage
        //Viewmodel action to view both the sales the user has made as well as the items they have currently listed
        public IActionResult UserPage()
        {
            {
                //Getting the current user then making so only items listed and items sold by current user are shown for their user page
                var seller = _userManager.GetUserName(User);
                var items = _context.Item
                .Where(m => m.Seller == seller);
                var sales = _context.Sales
                    .Where(m => m.SellerPerson == seller);

                //Creating the viewmodel to be returned
                UserPageViewModel vm = new UserPageViewModel();
                vm.Item = items;
                vm.Sale = sales;
                return View(vm);
            }
        }

        // GET: MyItems
        public IActionResult Index()
        {

             var seller = _userManager.GetUserName(User);
             var items = _context.Item
                  .Where(m => m.Seller == seller);
               return View("Index", items);
        
        }


        // GET: MyItems/Details/5
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

        // GET: MyItems/Create
        public IActionResult Create()
        {

            return View();
        }

        // POST: MyItems/Create

        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemId,Name,Description,Maker,Price,Quantity")] Item item)
        {
            if (ModelState.IsValid)
            {
                //Set variable seller (in the DB) as current logged on user
                var seller = _userManager.GetUserName(User);
                item.Seller = seller;
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(UserPage));
            }
            return View(item);
        }

        // GET: MyItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
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

        // POST: MyItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ItemId,Name,Description,Maker,Seller,Price,Quantity")] Item item)
        {
            if (id != item.ItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var seller = _userManager.GetUserName(User);
                    item.Seller = seller;
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
                return RedirectToAction(nameof(UserPage));
            }
            return View(item);
        }

        // GET: MyItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: MyItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Item.FindAsync(id);
            _context.Item.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(UserPage));
        }

        private bool ItemExists(int id)
        {
            return _context.Item.Any(e => e.ItemId == id);
        }
    }
}
