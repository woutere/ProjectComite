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
    public class GemeentenController : Controller
    {
        private readonly ComiteContext _context;

        public GemeentenController(ComiteContext context)
        {
            _context = context;
        }

        // GET: Gemeenten
        public async Task<IActionResult> Index()
        {
            return View(await _context.gemeenten.ToListAsync());
        }

        // GET: Gemeenten/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gemeente = await _context.gemeenten
                .FirstOrDefaultAsync(m => m.gemeenteId == id);
            if (gemeente == null)
            {
                return NotFound();
            }

            return View(gemeente);
        }

        // GET: Gemeenten/Create
        public IActionResult Create()
        {
            var viewmodel = new CreateGemeenteViewModel();
            viewmodel.gemeente = new Gemeente();
            viewmodel.leden = _context.leden.ToList();
            viewmodel.acties = new SelectList(_context.acties, "actieId", "naam");
            return View(viewmodel);
        }

        // POST: Gemeenten/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGemeenteViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(viewmodel.gemeente);
                await _context.SaveChangesAsync();
                foreach (var geselecteerdLid in viewmodel.leden)
                {
                    if (geselecteerdLid.CheckboxAnswer==true)
                    {
                        geselecteerdLid.gemeenteId = viewmodel.gemeente.gemeenteId;
                        Lid lid = _context.leden.SingleOrDefault(l => l.lidId == geselecteerdLid.lidId);
                        if (lid != null)
                        {
                            lid.gemeenteId = geselecteerdLid.gemeenteId;
                        }
                    }
                    
                }
            }
            return View(viewmodel);
        }

        // GET: Gemeenten/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gemeente = await _context.gemeenten.FindAsync(id);
            if (gemeente == null)
            {
                return NotFound();
            }
            return View(gemeente);
        }

        // POST: Gemeenten/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("gemeenteId,naam,postcode")] Gemeente gemeente)
        {
            if (id != gemeente.gemeenteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gemeente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GemeenteExists(gemeente.gemeenteId))
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
            return View(gemeente);
        }

        // GET: Gemeenten/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gemeente = await _context.gemeenten
                .FirstOrDefaultAsync(m => m.gemeenteId == id);
            if (gemeente == null)
            {
                return NotFound();
            }

            return View(gemeente);
        }

        // POST: Gemeenten/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gemeente = await _context.gemeenten.FindAsync(id);
            _context.gemeenten.Remove(gemeente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GemeenteExists(int id)
        {
            return _context.gemeenten.Any(e => e.gemeenteId == id);
        }
    }
}
