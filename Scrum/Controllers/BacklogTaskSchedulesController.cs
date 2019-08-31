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
    [Route("Products/backlogtaskschedule/{backlogtaskid:int}/[action]/{id:int?}")]
    public class BacklogTaskSchedulesController : Controller
    {
        private readonly ScrumContext _context;
        private readonly IAuthorizationService _authorizationService;

        private ProductBacklogItem BacklogItem;
        private BacklogTask BacklogItemTask;


        public BacklogTaskSchedulesController(ScrumContext context, IAuthorizationService authorizationService)
        {
            _context = context;
            _authorizationService = authorizationService;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            int id = int.Parse(context.RouteData.Values["backlogtaskid"].ToString());
            BacklogItemTask = await _context.BacklogTasks.Include(p => p.Schedule).Include(p=> p.Item).FirstOrDefaultAsync(p => p.Id == id);
           
            if (BacklogItemTask == null)
            {
                context.Result = new NotFoundResult();
                return;
            }
            else
            {
                BacklogItem = BacklogItemTask.Item;
                await next();
            }
        }

        // GET: BacklogTaskSchedules
        public async Task<IActionResult> Index()
        {
            var allowed = await _authorizationService.AuthorizeAsync(User, BacklogItem, Operations.View);
            if (!allowed.Succeeded)
            {
                return new ForbidResult();
            }

            return View(BacklogItemTask.Schedule);
        }

        // GET: BacklogTaskSchedules/Details/5
        public async Task<IActionResult> Details(int? id, DateTime day)
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

            var backlogTaskSchedule = await _context.BackLogTaskSchedules
                .Include(b => b.Task)
                .FirstOrDefaultAsync(m => m.BackLogTaskId == id && m.Day.Equals(day));
            if (backlogTaskSchedule == null)
            {
                return NotFound();
            }

            return View(backlogTaskSchedule);
        }

        // GET: BacklogTaskSchedules/Create
        public async Task<IActionResult> Create()
        {

            var allowed = await _authorizationService.AuthorizeAsync(User, BacklogItem, Operations.Update);
            if (!allowed.Succeeded)
            {
                return new ForbidResult();
            }
            ViewData["BackLogTaskId"] = BacklogItemTask.Id;
            return View();
        }

        // POST: BacklogTaskSchedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BackLogTaskId,Day,Hours")] BacklogTaskSchedule backlogTaskSchedule)
        {
            var allowed = await _authorizationService.AuthorizeAsync(User, BacklogItem, Operations.Update);
            if (!allowed.Succeeded)
            {
                return new ForbidResult();

            }

            if (ModelState.IsValid)
            {
                _context.Add(backlogTaskSchedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BackLogTaskId"] = BacklogItemTask.Id;
            return View(backlogTaskSchedule);
        }

        // GET: BacklogTaskSchedules/Edit/5
        public async Task<IActionResult> Edit(int? id, DateTime? day)
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

            var backlogTaskSchedule = await _context.BackLogTaskSchedules.FindAsync(id, day);
            if (backlogTaskSchedule == null)
            {
                return NotFound();
            }
            ViewData["BackLogTaskId"] = backlogTaskSchedule.BackLogTaskId;
            return View(backlogTaskSchedule);
        }

        // POST: BacklogTaskSchedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DateTime day, [Bind("BackLogTaskId,Day,Hours")] BacklogTaskSchedule backlogTaskSchedule)
        {
            if (id != backlogTaskSchedule.BackLogTaskId)
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
                    _context.Update(backlogTaskSchedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BacklogTaskScheduleExists(backlogTaskSchedule.BackLogTaskId, day))
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
            ViewData["BackLogTaskId"] = backlogTaskSchedule.BackLogTaskId;
            return View(backlogTaskSchedule);
        }

        // GET: BacklogTaskSchedules/Delete/5
        public async Task<IActionResult> Delete(int? id, DateTime day)
        {
            if (id == null || day == null)
            {
                return NotFound();
            }

            var allowed = await _authorizationService.AuthorizeAsync(User, BacklogItem, Operations.Update);
            if (!allowed.Succeeded)
            {
                return new ForbidResult();

            }

            var backlogTaskSchedule = await _context.BackLogTaskSchedules
                .Include(b => b.Task)
                .FirstOrDefaultAsync(m => m.BackLogTaskId == id && m.Day == day);
            if (backlogTaskSchedule == null)
            {
                return NotFound();
            }

            return View(backlogTaskSchedule);
        }

        // POST: BacklogTaskSchedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, DateTime day)
        {

            var allowed = await _authorizationService.AuthorizeAsync(User, BacklogItem, Operations.Update);
            if (!allowed.Succeeded)
            {
                return new ForbidResult();

            }

            var backlogTaskSchedule = await _context.BackLogTaskSchedules.FindAsync(id, day);
            _context.BackLogTaskSchedules.Remove(backlogTaskSchedule);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BacklogTaskScheduleExists(int id, DateTime day)
        {
            return _context.BackLogTaskSchedules.Any(e => e.BackLogTaskId == id && e.Day == day);
        }
    }
}
