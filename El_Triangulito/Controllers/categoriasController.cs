using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using El_Triangulito.Models;

namespace El_Triangulito.Controllers
{
    public class categoriasController : Controller
    {
        private readonly elTriangulitoDBcontex _context;

        public categoriasController(elTriangulitoDBcontex context)
        {
            _context = context;
        }

        // GET: categorias
        public async Task<IActionResult> Index()
        {
            // Mostrar mensajes de error si existen
            if (TempData["Error"] != null)
            {
                ViewBag.Error = TempData["Error"];
            }
            return View(await _context.categorias.ToListAsync());
        }

        // POST: categorias/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("categoria")] categorias categorias)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categorias);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Redirige al Index después de crear
            }

            // Si el modelo no es válido, regresa al Index con los errores
            TempData["Error"] = "Por favor, corrija los errores en el formulario.";
            return RedirectToAction(nameof(Index));
        }

        // POST: categorias/Edit/5
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