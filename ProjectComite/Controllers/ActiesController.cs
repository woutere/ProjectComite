﻿using System;
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

namespace ProjectComite
{
    [Authorize]
    public class ActiesController : Controller
    {
        private readonly ComiteContext _context;

        public ActiesController(ComiteContext context)
        {
            _context = context;
        }

        // GET: Acties
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var comiteContext = _context.acties.Include(a => a.gemeente);
            return View(await comiteContext.ToListAsync());
        }

        // GET: Acties/Details/5
        public async Task<IActionResult> Details(int? id, DetailActieViewModel viewmodel)
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
            viewmodel.actie = actie;
            viewmodel.leden = new List<Lid>(from s in _context.leden
                                               join ss in _context.actieleden on s.lidId equals ss.lidId
                                               where ss.actieId == id
                                               select s).ToList();

            return View(viewmodel);
        }

        // GET: Acties/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            CreateActieViewModel viewmodel = new CreateActieViewModel();
            viewmodel.actie = new Actie();
            viewmodel.gemeentes = new SelectList(_context.gemeenten, "gemeenteId", "naam");
            viewmodel.leden = _context.leden.ToList();
            return View(viewmodel);
        }

        // POST: Acties/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateActieViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                viewmodel.actie.leden = new List<ActieLid>();
                foreach (var actie in viewmodel.leden.Where(a => a.CheckboxAnswer == true))
                {
                    viewmodel.actie.leden.Add(new ActieLid() { lidId = actie.lidId });
                }
                _context.Add(viewmodel.actie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(viewmodel);
        }

        // GET: Acties/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            EditActieViewModel viewmodel = new EditActieViewModel();
            var actie = await _context.acties.FindAsync(id);
            if (actie == null)
            {
                return NotFound();
            }
            viewmodel.actie = actie;
            viewmodel.gemeentes = new SelectList(_context.gemeenten, "gemeenteId", "naam", actie.gemeenteId);
            viewmodel.leden = _context.leden.ToList();
            foreach (var lid in viewmodel.leden)
            {
                if (_context.actieleden.Any(al => al.actieId == id && al.lidId == lid.lidId))
                {
                    lid.CheckboxAnswer = true;
                }
            }
            return View(viewmodel);
        }

        // POST: Acties/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditActieViewModel viewmodel)
        {
            if (id != viewmodel.actie.actieId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(viewmodel.actie);
                    await _context.SaveChangesAsync();
                    var currentActieLeden = _context.actieleden.Where(al => al.lidId == viewmodel.actie.actieId).ToList();
                    for (int i = 0; i < viewmodel.leden.Count; i++)
                    {
                        if (viewmodel.leden[i].CheckboxAnswer == true)
                        {
                            if (!currentActieLeden.Any(al => al.lidId == viewmodel.leden[i].lidId && al.actieId == viewmodel.actie.actieId))
                            {
                                _context.Add(new ActieLid() { actieId = viewmodel.leden[i].lidId, lidId = viewmodel.actie.actieId });
                            }
                        }
                        if (viewmodel.leden[i].CheckboxAnswer == false)
                        {
                            if (currentActieLeden.Any(al => al.lidId == viewmodel.leden[i].lidId && al.actieId == viewmodel.actie.actieId))
                            {
                                _context.Remove(currentActieLeden.FirstOrDefault(al => al.lidId == viewmodel.leden[i].lidId && al.actieId == viewmodel.actie.actieId));
                            }
                        }
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActieExists(viewmodel.actie.actieId))
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

        // GET: Acties/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id,DeleteActieViewModel viewmodel)
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
            viewmodel.actie = actie;
            viewmodel.leden = new List<Lid>(from s in _context.leden
                                            join ss in _context.actieleden on s.lidId equals ss.lidId
                                            where ss.actieId == id
                                            select s).ToList();
            return View(viewmodel);
        }

        // POST: Acties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, DeleteActieViewModel viewmodel)
        {
            var actie = await _context.acties.FindAsync(id);
            _context.acties.Remove(actie);
            foreach (var lid in viewmodel.leden)
            {
                ActieLid actieLid = _context.actieleden.FirstOrDefault(al => al.lidId == lid.lidId && al.actieId == viewmodel.actie.actieId);
                _context.Remove(actieLid);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActieExists(int id)
        {
            return _context.acties.Any(e => e.actieId == id);
        }
    }
}
