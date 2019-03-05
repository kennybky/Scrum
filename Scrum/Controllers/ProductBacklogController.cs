using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Scrum.Data;
using Scrum.Services;

namespace Scrum.Controllers
{
    [Authorize]
    [Route("Products/backlog/{productid:int}/[action]")]
    public class ProductBacklogController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthorizationService _authorizationService;
        private Product Product;


        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            int id = int.Parse(context.RouteData.Values["productid"].ToString());
            Product =await  _context.Products.Include(p => p.ProductManager)
                .Include(p => p.ProductBacklog).FirstOrDefaultAsync(p => p.Id == id);
            var allowed = await _authorizationService.AuthorizeAsync(User, Product, ProductOperations.ViewBacklog);
            if (allowed.Succeeded)
            {
                await next();
            } else
            {
                context.Result = new ForbidResult();
            }
        }
       
        public ProductBacklogController(ApplicationDbContext context, IAuthorizationService authorizationService)
        {
            _context = context;
            _authorizationService = authorizationService;
        }

        // GET: Product/Backlog/1/index
        public IActionResult Index()
        {
            if (Product == null)
            {
                return NotFound();
            }
            return View(Product.ProductBacklog);
        }

        // GET: Product/Backlog/2/Details/5
        [Route("{id:int}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productBacklogItem = await _context.ProductBackLogItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productBacklogItem == null)
            {
                return NotFound();
            }

            return View(productBacklogItem);
        }

        // GET: Products/Backlog/5/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Backlog/5/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,Priority,Status")] ProductBacklogItem productBacklogItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productBacklogItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productBacklogItem);
        }

        // GET: Product/Backlog/1/Edit/5
        [Route("{id:int}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productBacklogItem = await _context.ProductBackLogItems.FindAsync(id);
            if (productBacklogItem == null)
            {
                return NotFound();
            }
            return View(productBacklogItem);
        }

        // POST: Product/Backlog/5/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("{id:int}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,Priority,Status")] ProductBacklogItem productBacklogItem)
        {
            if (id != productBacklogItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productBacklogItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductBacklogItemExists(productBacklogItem.Id))
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
            return View(productBacklogItem);
        }

        // GET: Product/Backlog/3/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productBacklogItem = await _context.ProductBackLogItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productBacklogItem == null)
            {
                return NotFound();
            }

            return View(productBacklogItem);
        }

        // POST: Product/Backlog/3/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productBacklogItem = await _context.ProductBackLogItems.FindAsync(id);
            _context.ProductBackLogItems.Remove(productBacklogItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductBacklogItemExists(int id)
        {
            return _context.ProductBackLogItems.Any(e => e.Id == id);
        }
    }
}
