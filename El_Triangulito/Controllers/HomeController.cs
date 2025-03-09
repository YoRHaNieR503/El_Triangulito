using El_Triangulito.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;

namespace El_Triangulito.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly elTriangulitoDBcontex _context;

        public HomeController(elTriangulitoDBcontex context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Métricas rápidas
            ViewBag.TotalCategorias = _context.categorias.Count();
            ViewBag.TotalCombos = _context.combos.Count();
            ViewBag.MesasOcupadas = _context.mesas.Count(m => m.id_estado == 1); // Suponiendo que 1 es "Ocupada"
            ViewBag.MesasDisponibles = _context.mesas.Count(m => m.id_estado == 0); // Suponiendo que 0 es "Disponible"
            ViewBag.PromocionesActivas = _context.Promociones
                .Count(p => p.fecha_inicio <= DateOnly.FromDateTime(DateTime.Now) &&
                              p.fecha_final >= DateOnly.FromDateTime(DateTime.Now));

            // Ventas por categoría
            var ventasPorCategoria = (from dp in _context.Detalle_Pedido
                                      join c in _context.Cuenta on dp.Id_cuenta equals c.Id_cuenta
                                      join im in _context.items_menu on dp.Id_plato equals im.id_item_menu
                                      join cat in _context.categorias on im.id_categoria equals cat.id_categoria
                                      where c.Estado_cuenta == "Entregado" // Solo cuentas entregadas
                                      group dp by cat.categoria into g
                                      select new
                                      {
                                          Categoria = g.Key,
                                          TotalVentas = g.Sum(dp => dp.Cantidad) // Sumar la cantidad de platos vendidos
                                      })
                                      .ToList();

            // Pasar los datos a la vista usando ViewBag
            ViewBag.VentasPorCategoria = ventasPorCategoria;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}