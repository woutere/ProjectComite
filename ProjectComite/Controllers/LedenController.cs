using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectComite.Models;
using ProjectComite.data;
using ProjectComite.ViewModels;

namespace ProjectComite.Controllers
{
    public class LedenController : Controller
    {
        private readonly ComiteContext _context;

        public LedenController(ComiteContext context)
        {
            _context = context;
        }

        // GET: Leden
        public async Task<IActionResult> Index()
        {
            var comiteContext = _context.leden.Include(l => l.gemeente);
            return View(await comiteContext.ToListAsync());
        }

        // GET: Leden/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var viewmodel = new DetailLidViewModel();
            viewmodel.lid = new Lid();

            viewmodel.lid = await _context.leden
                .Include(l => l.gemeente)
                .FirstOrDefaultAsync(m => m.lidId == id);
            if (viewmodel.lid == null)
            {
                return NotFound();
            }
            //viewmodel.acties = await _context.acties
            //    .Inclu

            return View(viewmodel);
            //Later nog op terugkomenn
        }

        // GET: Leden/Create
        public IActionResult Create()
        {
            var viewmodel = new CreateLidViewModel();
            viewmodel.lid = new Lid();
            viewmodel.gemeentes = new SelectList(_context.gemeenten);
            viewmodel.acties = new SelectList(_context.acties);
            return View(viewmodel);
        }

        // POST: Leden/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("lidId,naam,gemeenteId,lidgeldBetaald,emailAdres,telefoonnummer")] CreateLidViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(viewmodel.lid);
                _context.Add(viewmodel.acties);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["gemeenteId"] = new SelectList(_context.gemeenten, "gemeenteId", "gemeenteId", viewmodel.lid.gemeenteId);

            return View(viewmodel);
        }

        // GET: Leden/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lid = await _context.leden.FindAsync(id);
            if (lid == null)
            {
                return NotFound();
            }
            ViewData["gemeenteId"] = new SelectList(_context.gemeenten, "gemeenteId", "gemeenteId", lid.gemeenteId);
            return View(lid);
        }

        // POST: Leden/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("lidId,naam,gemeenteId,lidgeldBetaald,emailAdres,telefoonnummer")] Lid lid)
        {
            if (id != lid.lidId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lid);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LidExists(lid.lidId))
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
            ViewData["gemeenteId"] = new SelectList(_context.gemeenten, "gemeenteId", "gemeenteId", lid.gemeenteId);
            return View(lid);
        }

        // GET: Leden/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lid = await _context.leden
                .Include(l => l.gemeente)
                .FirstOrDefaultAsync(m => m.lidId == id);
            if (lid == null)
            {
                return NotFound();
            }

            return View(lid);
        }

        // POST: Leden/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lid = await _context.leden.FindAsync(id);
            _context.leden.Remove(lid);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LidExists(int id)
        {
            return _context.leden.Any(e => e.lidId == id);
        }
    }
}
