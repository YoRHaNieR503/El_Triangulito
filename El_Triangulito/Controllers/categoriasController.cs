﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ModuloMeseros.Models;

namespace ModuloMeseros.Controllers
{
    public class categoriasController : Controller
    {
        private readonly DulceSavorDbContext _context;

        public categoriasController(DulceSavorDbContext context)
        {
            _context = context;
        }

        // GET: categorias
        public async Task<IActionResult> Index()
        {
            return View(await _context.categorias.ToListAsync());
        }

        // GET: categorias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categorias = await _context.categorias
                .FirstOrDefaultAsync(m => m.id_categoria == id);
            if (categorias == null)
            {
                return NotFound();
            }

            return View(categorias);
        }

        // GET: categorias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: categorias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_categoria,categoria")] categorias categorias)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categorias);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categorias);
        }

        // GET: categorias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categorias = await _context.categorias.FindAsync(id);
            if (categorias == null)
            {
                return NotFound();
            }
            return View(categorias);
        }

        // POST: categorias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_categoria,categoria")] categorias categorias)
        {
            if (id != categorias.id_categoria)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categorias);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!categoriasExists(categorias.id_categoria))
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
            return View(categorias);
        }

        // GET: categorias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categorias = await _context.categorias
                .FirstOrDefaultAsync(m => m.id_categoria == id);
            if (categorias == null)
            {
                return NotFound();
            }

            return View(categorias);
        }

        // POST: categorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categorias = await _context.categorias.FindAsync(id);
            if (categorias != null)
            {
                _context.categorias.Remove(categorias);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool categoriasExists(int id)
        {
            return _context.categorias.Any(e => e.id_categoria == id);
        }
    }
}
