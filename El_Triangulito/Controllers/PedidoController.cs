using Microsoft.AspNetCore.Mvc;
using ModuloMeseros.Models;

namespace ModuloMeseros.Controllers
{
    public class PedidoController : Controller
    {

        private readonly DulceSavorDbContext _context;

        public PedidoController(DulceSavorDbContext context)
        {
            _context = context;
        }


        public IActionResult Index(int id_categoria, int idMesa)
        {

            var listaProductos = (from ca in _context.categorias
                                  join im in _context.items_menu on ca.id_categoria equals im.id_categoria
                                  join e in _context.estados on im.id_estado equals e.id_estado
                                  where im.id_categoria == id_categoria
                                  && e.tipo_estado == "disponibilidad"
                                  select new
                                  {
                                      Producto = im.nombre,
                                      Estado = e.nombre,
                                      Precio = im.precio,
                                      IdPlato = im.id_item_menu

                                  }).ToList();
            ViewData["listaProductos"] = listaProductos;


            var categoria = _context.categorias.FirstOrDefault(m => m.id_categoria == id_categoria);

            if (categoria == null)
            {
                return NotFound(); // Devuelve una respuesta HTTP 404 - Not Found
            }

            ViewData["Categoria"] = categoria.categoria;

            var mesa = _context.mesas.FirstOrDefault(m => m.id_mesa == idMesa);

            if (mesa == null)
            {
                // Si la mesa no se encuentra en la base de datos.
                return NotFound(); // Devuelve una respuesta HTTP 404 - Not Found
            }

            // Pasar los detalles de la mesa a la vista
            ViewData["IdMesa"] = mesa.id_mesa;

            var id_cuenta = (from c in _context.Cuenta
                             where c.Id_mesa == idMesa
                             select c.Id_cuenta).Take(1).ToList();

            ViewData["idCuenta"] = id_cuenta;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AgregarPedido(int idMesa, int idCuenta, int idPlato, int cantidad, string Estad, decimal totalPedido)
        {
            var nuevoProducto = new Detalle_Pedido
            {
                Id_cuenta = idCuenta,
                Id_plato = idPlato,
                Cantidad = cantidad,
                Estado = Estad,
                Tipo_Plato = 'P',
                Precio = totalPedido
                

            };

            _context.Detalle_Pedido.Add(nuevoProducto);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Detalles", new { idMesaCuenta = idMesa });
        }




    }
}
