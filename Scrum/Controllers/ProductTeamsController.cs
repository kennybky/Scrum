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
    public class ProductTeamsController : Controller
    {
        private readonly ScrumContext _context;

        public ProductTeamsController(ScrumContext context)
        {
            _context = context;
        }

        // GET: ProductTeams
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ProductTeams.Include(p => p.Product).Include(p => p.Team);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ProductTeams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productTeam = await _context.ProductTeams
                .Include(p => p.Product)
                .Include(p => p.Team)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (productTeam == null)
            {
                return NotFound();
            }

            return View(productTeam);
        }

        // GET: ProductTeams/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Description");
            ViewData["TeamId"] = new SelectList(_context.ScrumTeams, "Id", "TeamName");
            return View();
        }

        // POST: ProductTeams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,TeamId")] ProductTeam productTeam)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productTeam);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Description", productTeam.ProductId);
            ViewData["TeamId"] = new SelectList(_context.ScrumTeams, "Id", "TeamName", productTeam.TeamId);
            return View(productTeam);
        }

        // GET: ProductTeams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productTeam = await _context.ProductTeams.FindAsync(id);
            if (productTeam == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Description", productTeam.ProductId);
            ViewData["TeamId"] = new SelectList(_context.ScrumTeams, "Id", "TeamName", productTeam.TeamId);
            return View(productTeam);
        }

        // POST: ProductTeams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,TeamId")] ProductTeam productTeam)
        {
            if (id != productTeam.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productTeam);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductTeamExists(productTeam.ProductId))
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
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Description", productTeam.ProductId);
            ViewData["TeamId"] = new SelectList(_context.ScrumTeams, "Id", "TeamName", productTeam.TeamId);
            return View(productTeam);
        }

        // GET: ProductTeams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productTeam = await _context.ProductTeams
                .Include(p => p.Product)
                .Include(p => p.Team)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (productTeam == null)
            {
                return NotFound();
            }

            return View(productTeam);
        }

        // POST: ProductTeams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productTeam = await _context.ProductTeams.FindAsync(id);
            _context.ProductTeams.Remove(productTeam);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductTeamExists(int id)
        {
            return _context.ProductTeams.Any(e => e.ProductId == id);
        }
    }
}
