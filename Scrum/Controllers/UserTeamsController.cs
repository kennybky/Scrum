using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Scrum.Data;

namespace Scrum.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class UserTeamsController : Controller
    {
        private readonly ScrumContext _context;

        public UserTeamsController(ScrumContext context)
        {
            _context = context;
        }

        // GET: UserTeams
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ScrumUserTeams.Include(s => s.Team).Include(s => s.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: UserTeams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scrumUserTeam = await _context.ScrumUserTeams
                .Include(s => s.Team)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (scrumUserTeam == null)
            {
                return NotFound();
            }

            return View(scrumUserTeam);
        }

        // GET: UserTeams/Create
        public IActionResult Create()
        {
            ViewData["TeamId"] = new SelectList(_context.ScrumTeams, "Id", "TeamName");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FirstName");
            return View();
        }

        // POST: UserTeams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,TeamId")] ScrumUserTeam scrumUserTeam)
        {
            if (ModelState.IsValid)
            {
                _context.Add(scrumUserTeam);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TeamId"] = new SelectList(_context.ScrumTeams, "Id", "TeamName", scrumUserTeam.TeamId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FirstName", scrumUserTeam.UserId);
            return View(scrumUserTeam);
        }

        // GET: UserTeams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scrumUserTeam = await _context.ScrumUserTeams.FindAsync(id);
            if (scrumUserTeam == null)
            {
                return NotFound();
            }
            ViewData["TeamId"] = new SelectList(_context.ScrumTeams, "Id", "TeamName", scrumUserTeam.TeamId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FirstName", scrumUserTeam.UserId);
            return View(scrumUserTeam);
        }

        // POST: UserTeams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,TeamId")] ScrumUserTeam scrumUserTeam)
        {
            if (id != scrumUserTeam.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(scrumUserTeam);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScrumUserTeamExists(scrumUserTeam.UserId))
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
            ViewData["TeamId"] = new SelectList(_context.ScrumTeams, "Id", "TeamName", scrumUserTeam.TeamId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FirstName", scrumUserTeam.UserId);
            return View(scrumUserTeam);
        }

        // GET: UserTeams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scrumUserTeam = await _context.ScrumUserTeams
                .Include(s => s.Team)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (scrumUserTeam == null)
            {
                return NotFound();
            }

            return View(scrumUserTeam);
        }

        // POST: UserTeams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var scrumUserTeam = await _context.ScrumUserTeams.FindAsync(id);
            _context.ScrumUserTeams.Remove(scrumUserTeam);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScrumUserTeamExists(int id)
        {
            return _context.ScrumUserTeams.Any(e => e.UserId == id);
        }
    }
}
