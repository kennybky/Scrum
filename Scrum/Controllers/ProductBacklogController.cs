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
    [Route("Products/backlog/{productid:int}/[action]/{id:int?}")]
    public class ProductBacklogController : Controller
    {
        private readonly ScrumContext _context;
        private readonly IAuthorizationService _authorizationService;
        private Product Product;


        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            int id = int.Parse(context.RouteData.Values["productid"].ToString());
            Product = await _context.Products.Include(p => p.ProductManager)
                .Include(p => p.ProductBacklog).FirstOrDefaultAsync(p => p.Id == id);
            if (Product == null)
            {
                context.Result = new NotFoundResult();
                return;
            }
            else
            {
                await next();
            }
        }

        public ProductBacklogController(ScrumContext context, IAuthorizationService authorizationService)
        {
            _context = context;
            _authorizationService = authorizationService;
        }

        // GET: Product/Backlog/1/index
        public async Task<IActionResult> Index()
        {
            var allowed = await _authorizationService.AuthorizeAsync(User, Product, Operations.View);
            if (!allowed.Succeeded)
            {
                return new ForbidResult();
            }
            return View(Product.ProductBacklog);
        }

        // GET: Product/Backlog/2/Details/5
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

            var allowed = await _authorizationService.AuthorizeAsync(User, productBacklogItem, Operations.View);
            if (!allowed.Succeeded)
            {
                return new ForbidResult();
            }
            

            return View(productBacklogItem);
        }

        // GET: Products/Backlog/5/Create
        public async Task<IActionResult> Create()
        {
            var allowed = await _authorizationService.AuthorizeAsync(User, Product, Operations.Create);
            if(!allowed.Succeeded)
            {
                return new ForbidResult();
            }
            return View();
        }

        // POST: Products/Backlog/5/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,Priority,Status")] ProductBacklogItem productBacklogItem)
        {
            var allowed = await _authorizationService.AuthorizeAsync(User, Product, Operations.Create);
            if (!allowed.Succeeded)
            {
                return new ForbidResult();
            }

            if (ModelState.IsValid)
            {
                _context.Add(productBacklogItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productBacklogItem);
        }

        // GET: Product/Backlog/1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            var allowed = await _authorizationService.AuthorizeAsync(User, Product, Operations.Manage);
            if (!allowed.Succeeded)
            {
                return new ForbidResult();
            }

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,Priority,Status")] ProductBacklogItem productBacklogItem)
        {
            if (id != productBacklogItem.Id)
            {
                return NotFound();
            }

            var allowed = await _authorizationService.AuthorizeAsync(User, Product, Operations.Manage);
            if (!allowed.Succeeded)
            {
                return new ForbidResult();
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
            var allowed = await _authorizationService.AuthorizeAsync(User, Product, Operations.Delete);
            if (!allowed.Succeeded)
            {
                return new ForbidResult();
            }
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
