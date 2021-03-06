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

namespace ProjectComite.Controllers
{
    [Authorize]
    public class GemeentenController : Controller
    {
        private readonly ComiteContext _context;

        public GemeentenController(ComiteContext context)
        {
            _context = context;
        }
        [AllowAnonymous]
        // GET: Gemeenten
        public async Task<IActionResult> Index()
        {
            return View(await _context.gemeenten.ToListAsync());
        }
        [AllowAnonymous]
        // GET: Gemeenten/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            DetailGemeenteViewModel viewmodel = new DetailGemeenteViewModel();
            var gemeente = await _context.gemeenten
                .FirstOrDefaultAsync(m => m.gemeenteId == id);
            if (gemeente == null)
            {
                return NotFound();
            }
            viewmodel.gemeente = gemeente;
            viewmodel.acties = new List<Actie>();
            List<Actie> acties = _context.acties.ToList();
            foreach (var actie in acties)
            {
                if (_context.acties.Any(a => a.gemeenteId == id && a.actieId == actie.actieId))
                {
                    viewmodel.acties.Add(actie);
                }
            }
            viewmodel.leden = new List<Lid>();
            List<Lid> leden = _context.leden.ToList();
            foreach (var lid in leden)
            {
                if (_context.leden.Any(l => l.gemeenteId == id && l.lidId == lid.lidId))
                {
                    viewmodel.leden.Add(lid);
                }
            }
            return View(viewmodel);
        }
        [Authorize(Roles = "Admin")]
        // GET: Gemeenten/Create
        public IActionResult Create()
        {
            var viewmodel = new CreateGemeenteViewModel();
            viewmodel.gemeente = new Gemeente();
            viewmodel.leden = _context.leden.ToList();
            viewmodel.acties = _context.acties.ToList();
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
                return RedirectToAction(nameof(Index));
                //Gemeente gemeente = _context.gemeenten.SingleOrDefault(x => x.gemeenteId == viewmodel.gemeente.gemeenteId);
                //viewmodel.gemeente.leden = new List<Lid>();
                //foreach (Lid lid in viewmodel.leden)
                //{
                //    if (lid.CheckboxAnswer == true)
                //    {
                //        viewmodel.gemeente.leden.Add(lid);
                //    }

                //}
                //viewmodel.gemeente.acties = new List<Actie>();
                //foreach (Actie actie in viewmodel.acties)
                //{
                //    if (actie.CheckboxAnswer == true)
                //    {
                //        viewmodel.gemeente.acties.Add(actie);
                //    }

                //}
                //_context.Add(viewmodel.gemeente);
                //await _context.SaveChangesAsync();
            }
            return View(viewmodel);
        }
        [Authorize(Roles = "Admin")]
        // GET: Gemeenten/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            EditGemeenteViewModel viewmodel= new EditGemeenteViewModel();
            var gemeente = await _context.gemeenten.FindAsync(id);
            if (gemeente == null)
            {
                return NotFound();
            }
            viewmodel.gemeente = gemeente;
            viewmodel.acties = _context.acties.ToList();
            foreach (var actie in viewmodel.acties)
            {
                if (_context.acties.Any(a=>a.gemeenteId==id&& a.actieId==actie.actieId))
                {
                    actie.CheckboxAnswer = true;
                }
            }
            viewmodel.leden = _context.leden.ToList();
            foreach (var lid in viewmodel.leden)
            {
                if (_context.leden.Any(l => l.gemeenteId == id&& l.lidId==lid.lidId))
                {
                    lid.CheckboxAnswer = true;
                }
            }
            return View(viewmodel);
        }

        // POST: Gemeenten/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditGemeenteViewModel viewmodel)
        {
            if (id != viewmodel.gemeente.gemeenteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    viewmodel.gemeente.leden = new List<Lid>();
                    foreach (Lid lid in viewmodel.leden)
                    {
                        if (lid.CheckboxAnswer == true)
                        {
                            lid.gemeenteId = id;
                            viewmodel.gemeente.leden.Add(lid);
                        }

                    }
                    viewmodel.gemeente.acties = new List<Actie>();
                    foreach (Actie actie in viewmodel.acties)
                    {
                        if (actie.CheckboxAnswer == true)
                        {
                            actie.gemeenteId = id;
                            viewmodel.gemeente.acties.Add(actie);
                        }

                    }
                    _context.Update(viewmodel.gemeente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GemeenteExists(viewmodel.gemeente.gemeenteId))
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
        [Authorize(Roles ="Admin")]
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
            gemeente.leden=new List<Lid>(from l in _context.leden where l.lidId== gemeente.gemeenteId select l).ToList();
            gemeente.acties = new List<Actie>(from a in _context.acties where a.actieId == gemeente.gemeenteId select a).ToList();
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
