using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModuloMeseros.Models;
using System.Linq;

namespace ModuloMeseros.Controllers
{
    public class DetallesController : Controller
    {
        private readonly elTriangulitoDBcontex _context;

        public DetallesController(elTriangulitoDBcontex context)
        {
            _context = context;
        }

        public IActionResult Index(int idMesaCuenta)
        {



            var listaEstados = (from e in _context.estados
                                join m in _context.mesas on e.id_estado equals m.id_estado
                                join cue in _context.Cuenta on m.id_mesa equals cue.Id_mesa
                                where cue.Id_mesa == idMesaCuenta
                                select Convert.ToInt32(cue.Estado_cuenta)).Take(1).ToList();

            ViewData["Estados"] = listaEstados;

            int idEstado = listaEstados.FirstOrDefault();

            var estado = _context.estados.FirstOrDefault(m => m.id_estado == idEstado);

            if (estado == null)
            {
                return NotFound();
            }

            ViewData["NombreEstado"] = estado.nombre;

            var mesa = _context.mesas.FirstOrDefault(m => m.id_mesa == idMesaCuenta);

            if (mesa == null)
            {
                return NotFound();
            }

            ViewData["IdMesa"] = mesa.id_mesa;
            ViewData["NumeroMesa"] = mesa.nombre_mesa;
            ViewData["CantidadPersonas"] = mesa.cantidad_personas;

            var listadoDetallePedidos = (from dp in _context.Detalle_Pedido
                                         join cue in _context.Cuenta on dp.Id_cuenta equals cue.Id_cuenta
                                         join me in _context.mesas on cue.Id_mesa equals me.id_mesa
                                         join im in _context.items_menu on dp.Id_plato equals im.id_item_menu
                                         where cue.Id_mesa == idMesaCuenta
                                         select new
                                         {
                                             Cantidad = dp.Cantidad,
                                             Pedido = im.nombre,
                                             Precio = dp.Precio,
                                             TipoPlato = dp.Tipo_Plato,
                                             IDCuenta = dp.Id_DetalleCuenta,
                                             IdPlato = dp.Id_plato,
                                             Nombre = cue.Nombre,
                                             Id_DetalleCuenta = dp.Id_DetalleCuenta
                                         }).ToList();

            ViewData["ListadoDetallePedido"] = listadoDetallePedidos;



            var nombreCliente = _context.Cuenta.FirstOrDefault(m => m.Id_mesa == idMesaCuenta);

            if (nombreCliente == null)
            {
                return NotFound();
            }

            ViewData["NombreCliente"] = nombreCliente.Nombre;

            var idCuenta = _context.Cuenta.Where(m => m.Id_mesa == idMesaCuenta).Select(c => c.Id_cuenta).FirstOrDefault();

            ViewData["idCuenta"] = idCuenta;


            var totalPrecio = (from dp in _context.Detalle_Pedido
                               join cue in _context.Cuenta on dp.Id_cuenta equals cue.Id_cuenta
                               join me in _context.mesas on cue.Id_mesa equals me.id_mesa
                               join im in _context.items_menu on dp.Id_plato equals im.id_item_menu
                               where cue.Id_mesa == idMesaCuenta
                               select dp.Precio).Sum();
            ViewData["TotalPrecio"] = totalPrecio;

            return View();
        }

        [HttpPost]
        public IActionResult EliminarProducto(int idDetallePedido)
        {
            var detallePedido = _context.Detalle_Pedido.Find(idDetallePedido);
            if (detallePedido != null)
            {
                _context.Detalle_Pedido.Remove(detallePedido);
                _context.SaveChanges();
                return Ok(); // Si la eliminación fue exitosa
            }
            return NotFound(); // Si el producto no se encontró
        }


        [HttpPost]
        public async Task<IActionResult> Ordenar(int IdCuenta, int idMesaCuenta)
        {
            // Obtener la cuenta por su ID
            var cuenta = await _context.Cuenta.FindAsync(IdCuenta);

            if (cuenta != null)
            {
                // Cambiar el estado de la cuenta
                cuenta.Estado_cuenta = "3"; // Cambiar el estado a "Solicitado" o al número que represente ese estado
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { idMesaCuenta = idMesaCuenta });
            }
            else
            {
                return NotFound();
            }
        }


        [HttpPost]
        public async Task<IActionResult> CancelarCuenta(int idMesaCuenta)
        {
            var cuenta = await _context.Cuenta.FirstOrDefaultAsync(c => c.Id_mesa == idMesaCuenta);

            if (cuenta != null)
            {
                // Borrar los detalles de pedido asociados a la cuenta
                var detallesPedido = await _context.Detalle_Pedido.Where(dp => dp.Id_cuenta == cuenta.Id_cuenta).ToListAsync();

                foreach (var detalle in detallesPedido)
                {
                    _context.Detalle_Pedido.Remove(detalle);
                }

                await _context.SaveChangesAsync(); // Guardar cambios antes de borrar la cuenta

                // Borrar la cuenta después de borrar los detalles de pedido
                _context.Cuenta.Remove(cuenta);

                // Cambiar el estado de la mesa a "Libre"
                var mesa = await _context.mesas.FirstOrDefaultAsync(m => m.id_mesa == idMesaCuenta);
                if (mesa != null)
                {
                    mesa.id_estado = 2; // Cambiar al estado "Libre" (2 es un ejemplo, asegúrate de usar el ID correcto para "Libre")
                }

                await _context.SaveChangesAsync(); // Guardar cambios después de cambiar el estado de la mesa

                return RedirectToAction("Index", "Mesas");
            }

            return NotFound();
        }






    }
}
