using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Scrum.Data;
using Scrum.Services;

namespace Scrum.Controllers
{
    [Authorize]
    public class ScrumTeamsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthorizationService _authorizationService;


        public ScrumTeamsController(ApplicationDbContext context, IAuthorizationService authorizationService)
        {
            _context = context;
            _authorizationService = authorizationService;
        }

        // GET: ScrumTeams
        public async Task<IActionResult> Index()
        {
            return View(await _context.ScrumTeams.ToListAsync());
        }

        // GET: ScrumTeams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var scrumTeam = await _context.ScrumTeams.Include(t => t.ScrumMaster).Include(t=> t.SprintBackLog).Include(t => t.ProductTeams).ThenInclude(pt=> pt.Product)
               .FirstOrDefaultAsync(m => m.Id == id);
            if (scrumTeam == null)
            {
                return NotFound();
            }

            var allowed = await _authorizationService.AuthorizeAsync(User, scrumTeam, Operations.View);
            if (!allowed.Succeeded)
            {
                return new ForbidResult();
            }
           

            return View(scrumTeam);
        }

        [Authorize(Roles=Roles.Admin)]
        // GET: ScrumTeams/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ScrumTeams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Create([Bind("Id,TeamName")] ScrumTeam scrumTeam)
        {
            if (ModelState.IsValid)
            {
                _context.Add(scrumTeam);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(scrumTeam);
        }

        [Authorize(Roles = Roles.Admin)]
        // GET: ScrumTeams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scrumTeam = await _context.ScrumTeams.FindAsync(id);
            if (scrumTeam == null)
            {
                return NotFound();
            }
            return View(scrumTeam);
        }

        // POST: ScrumTeams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TeamName")] ScrumTeam scrumTeam)
        {
            if (id != scrumTeam.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(scrumTeam);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScrumTeamExists(scrumTeam.Id))
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
            return View(scrumTeam);
        }

        // GET: ScrumTeams/Delete/5
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scrumTeam = await _context.ScrumTeams
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scrumTeam == null)
            {
                return NotFound();
            }

            return View(scrumTeam);
        }

        // POST: ScrumTeams/Delete/5
        [Authorize(Roles = Roles.Admin)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var scrumTeam = await _context.ScrumTeams.FindAsync(id);
            _context.ScrumTeams.Remove(scrumTeam);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScrumTeamExists(int id)
        {
            return _context.ScrumTeams.Any(e => e.Id == id);
        }
    }
}
