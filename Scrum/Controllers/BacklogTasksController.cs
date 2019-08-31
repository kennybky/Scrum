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
    [Route("Products/backlogtask/{backlogid:int}/[action]/{id:int?}")]
    public class BacklogTasksController : Controller
    {
        private readonly ScrumContext _context;

        private readonly IAuthorizationService _authorizationService;

        private ProductBacklogItem BacklogItem;

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            int id = int.Parse(context.RouteData.Values["backlogid"].ToString());
            BacklogItem = await _context.ProductBackLogItems.Include(p => p.Team).ThenInclude(t => t.UserTeams)
                .Include(p => p.Tasks).FirstOrDefaultAsync(p => p.Id == id);
            if (BacklogItem == null)
            {
                context.Result = new NotFoundResult();
                return;
            }
            else
            {
                await next();
            }
        }

        public BacklogTasksController(ScrumContext context, IAuthorizationService authorizationService)
        {
            _context = context;
            _authorizationService = authorizationService;
        }

        // GET: BacklogTasks
        public async Task<IActionResult> Index()
        {
            var allowed = await _authorizationService.AuthorizeAsync(User, BacklogItem, Operations.View);
            if (!allowed.Succeeded)
            {
                return new ForbidResult();
            }
            return View(BacklogItem.Tasks);
        }

        // GET: BacklogTasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allowed = await _authorizationService.AuthorizeAsync(User, BacklogItem, Operations.View);
            if (!allowed.Succeeded)
            {
                return new ForbidResult();
            }

            var backlogTask = await _context.BacklogTasks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (backlogTask == null)
            {
                return NotFound();
            }

            return View(backlogTask);
        }

        // GET: BacklogTasks/Create
        public async Task<IActionResult> Create()
        {
            var allowed = await _authorizationService.AuthorizeAsync(User, BacklogItem, Operations.Update);
            if (!allowed.Succeeded)
            {
                return new ForbidResult();
            }
            return View();
        }

        // POST: BacklogTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description")] BacklogTask backlogTask)
        {
            var allowed = await _authorizationService.AuthorizeAsync(User, BacklogItem, Operations.Update);
            if (!allowed.Succeeded)
            {
                return new ForbidResult();
            }
            if (ModelState.IsValid)
            {
                _context.Add(backlogTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(backlogTask);
        }

        // GET: BacklogTasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var allowed = await _authorizationService.AuthorizeAsync(User, BacklogItem, Operations.Update);
            if (!allowed.Succeeded)
            {
                return new ForbidResult();
            }

            var backlogTask = await _context.BacklogTasks.FindAsync(id);
            if (backlogTask == null)
            {
                return NotFound();
            }
            return View(backlogTask);
        }

        // POST: BacklogTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description")] BacklogTask backlogTask)
        {
            if (id != backlogTask.Id)
            {
                return NotFound();
            }


            var allowed = await _authorizationService.AuthorizeAsync(User, BacklogItem, Operations.Update);
            if (!allowed.Succeeded)
            {
                return new ForbidResult();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(backlogTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BacklogTaskExists(backlogTask.Id))
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
            return View(backlogTask);
        }

        // GET: BacklogTasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var allowed = await _authorizationService.AuthorizeAsync(User, BacklogItem, Operations.Update);
            if (!allowed.Succeeded)
            {
                return new ForbidResult();
            }

            var backlogTask = await _context.BacklogTasks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (backlogTask == null)
            {
                return NotFound();
            }

            return View(backlogTask);
        }

        // POST: BacklogTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {


            var allowed = await _authorizationService.AuthorizeAsync(User, BacklogItem, Operations.Update);
            if (!allowed.Succeeded)
            {
                return new ForbidResult();
            }

            var backlogTask = await _context.BacklogTasks.FindAsync(id);
            _context.BacklogTasks.Remove(backlogTask);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BacklogTaskExists(int id)
        {
            return _context.BacklogTasks.Any(e => e.Id == id);
        }
    }
}
