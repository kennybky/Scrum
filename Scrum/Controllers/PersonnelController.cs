using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scrum.Data;

namespace Scrum.Controllers
{

    // Changing user roles and adding data

    [Authorize(Roles=Roles.Admin)]
    public class PersonnelController : Controller
    {

        private readonly ScrumContext _context;

        public PersonnelController(ScrumContext context)
        {
            _context = context;
        }
        // GET: Personnel
        public ActionResult Index()
        {
            return View();
        }

        // GET: Personnel/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Personnel/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Personnel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Personnel/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Personnel/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Personnel/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Personnel/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}