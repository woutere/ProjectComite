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
using Microsoft.AspNetCore.Authorization;

namespace ProjectComite.Controllers
{
    [Authorize]
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

            Lid lid = await _context.leden
                .Include(l => l.gemeente)
                .FirstOrDefaultAsync(m => m.lidId == id);
            if (lid == null)
            {
                return NotFound();
            }
            viewmodel.lid = lid;
            viewmodel.acties = new List<Actie>(from s in _context.acties
                                               join ss in _context.actieleden on s.actieId equals ss.actieId
                                               where ss.lidId == id
                                               select s).ToList();
            return View(viewmodel);
        }
        [Authorize(Roles = "Admin")]
        // GET: Leden/Create
        public IActionResult Create()
        {
            CreateLidViewModel viewmodel = new CreateLidViewModel();
            viewmodel.lid = new Lid();
            viewmodel.gemeentes = new SelectList(_context.gemeenten, "gemeenteId", "naam");
            viewmodel.acties = _context.acties.ToList();
            return View(viewmodel);
        }

        // POST: Leden/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateLidViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                viewmodel.lid.actieleden = new List<ActieLid>();

                foreach (var actie in viewmodel.acties.Where(a => a.CheckboxAnswer == true))
                {
                    viewmodel.lid.actieleden.Add(new ActieLid() { actieId = actie.actieId });
                }
                _context.Add(viewmodel.lid);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(viewmodel);
        }
        [Authorize(Roles = "Admin")]
        // GET: Leden/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var viewmodel = new EditLidViewModel();
            viewmodel.lid = await _context.leden.FindAsync(id);
            if (viewmodel.lid == null)
            {
                return NotFound();
            }
            viewmodel.gemeentes = new SelectList(_context.gemeenten, "gemeenteId", "naam", viewmodel.lid.gemeenteId);
            viewmodel.acties = _context.acties.ToList();
            foreach (var actie in viewmodel.acties)
            {
                if (_context.actieleden.Any(al=>al.lidId== id && al.actieId==actie.actieId))
                {
                    actie.CheckboxAnswer = true;
                }
            }
            return View(viewmodel);
        }

        // POST: Leden/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,EditLidViewModel viewmodel)
        {
            if (id != viewmodel.lid.lidId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(viewmodel.lid);
                    await _context.SaveChangesAsync();

                    var currentActieLeden = _context.actieleden.Where(al => al.lidId == viewmodel.lid.lidId).ToList();
                    for (int i = 0; i < viewmodel.acties.Count; i++)
                    {
                        if (viewmodel.acties[i].CheckboxAnswer == true)
                        {
                            if (!currentActieLeden.Any(al => al.actieId == viewmodel.acties[i].actieId && al.lidId == viewmodel.lid.lidId))
                            {
                                _context.Add(new ActieLid() { actieId = viewmodel.acties[i].actieId, lidId = viewmodel.lid.lidId });
                            }
                        }
                        if (viewmodel.acties[i].CheckboxAnswer == false)
                        {
                            if (currentActieLeden.Any(al => al.actieId == viewmodel.acties[i].actieId && al.lidId == viewmodel.lid.lidId))
                            {
                                _context.Remove(currentActieLeden.FirstOrDefault(al => al.actieId == viewmodel.acties[i].actieId && al.lidId == viewmodel.lid.lidId));
                            }
                        }
                    }
                    viewmodel.gemeentes = new SelectList(_context.gemeenten, "gemeenteId", "naam", viewmodel.lid.gemeenteId);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LidExists(viewmodel.lid.lidId))
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

            return View(viewmodel);
        }
        [Authorize(Roles = "Admin")]
        // GET: Leden/Delete/5
        public async Task<IActionResult> Delete(int? id, DeleteLidViewModel viewmodel)
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
            viewmodel.lid = lid;
            viewmodel.acties = new List<Actie>(from s in _context.acties
                                               join ss in _context.actieleden on s.actieId equals ss.actieId
                                               where ss.lidId == id
                                               select s).ToList();
            return View(viewmodel);
        }

        // POST: Leden/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, DeleteLidViewModel viewmodel)
        {
            viewmodel.lid = await _context.leden.FindAsync(id);
            _context.leden.Remove(viewmodel.lid);
            viewmodel.acties =new List<Actie>(from s in _context.acties
                                             join ss in _context.actieleden on s.actieId equals ss.actieId
                                             where ss.lidId == id
                                             select s).ToList();
            if (!viewmodel.acties.Any())
            {

            }
            foreach (var actie in viewmodel.acties)
            {
                ActieLid actieLid = _context.actieleden.FirstOrDefault(al => al.actieId == actie.actieId && al.lidId == viewmodel.lid.lidId);
                _context.Remove(actieLid);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LidExists(int id)
        {
            return _context.leden.Any(e => e.lidId == id);
        }
    }
}
