using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectComite.Models;
using ProjectComite.data;

namespace ProjectComite
{
    public class ActiesController : Controller
    {
        private readonly ComiteContext _context;

        public ActiesController(ComiteContext context)
        {
            _context = context;
        }

        // GET: Acties
        public async Task<IActionResult> Index()
        {
            var comiteContext = _context.acties.Include(a => a.gemeente);
            return View(await comiteContext.ToListAsync());
        }

        // GET: Acties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actie = await _context.acties
                .Include(a => a.gemeente)
                .FirstOrDefaultAsync(m => m.actieId == id);
            if (actie == null)
            {
                return NotFound();
            }

            return View(actie);
        }

        // GET: Acties/Create
        public IActionResult Create()
        {
            ViewData["GemeenteId"] = new SelectList(_context.gemeenten, "gemeenteId", "gemeenteId");
            return View();
        }

        // POST: Acties/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("actieId,informatie,GemeenteId")] Actie actie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(actie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GemeenteId"] = new SelectList(_context.gemeenten, "gemeenteId", "gemeenteId", actie.GemeenteId);
            return View(actie);
        }

        // GET: Acties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actie = await _context.acties.FindAsync(id);
            if (actie == null)
            {
                return NotFound();
            }
            ViewData["GemeenteId"] = new SelectList(_context.gemeenten, "gemeenteId", "gemeenteId", actie.GemeenteId);
            return View(actie);
        }

        // POST: Acties/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("actieId,informatie,GemeenteId")] Actie actie)
        {
            if (id != actie.actieId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActieExists(actie.actieId))
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
            ViewData["GemeenteId"] = new SelectList(_context.gemeenten, "gemeenteId", "gemeenteId", actie.GemeenteId);
            return View(actie);
        }

        // GET: Acties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actie = await _context.acties
                .Include(a => a.gemeente)
                .FirstOrDefaultAsync(m => m.actieId == id);
            if (actie == null)
            {
                return NotFound();
            }

            return View(actie);
        }

        // POST: Acties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actie = await _context.acties.FindAsync(id);
            _context.acties.Remove(actie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActieExists(int id)
        {
            return _context.acties.Any(e => e.actieId == id);
        }
    }
}
