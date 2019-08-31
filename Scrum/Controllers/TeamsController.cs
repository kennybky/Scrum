using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Scrum.Data;

namespace Scrum.Controllers
{
    public class TeamsController : Controller
    {
        private readonly ScrumContext _context;

        public TeamsController(ScrumContext context)
        {
            _context = context;
        }

        // GET: Teams
        public async Task<IActionResult> Index()
        {
            return View(await _context.ScrumTeams.ToListAsync());
        }

        // GET: Teams/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Teams/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // GET: Teams/Edit/5
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

        // POST: Teams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // GET: Teams/Delete/5
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

        // POST: Teams/Delete/5
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
