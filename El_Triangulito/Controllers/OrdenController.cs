﻿using El_Triangulito.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.Arm;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace El_Triangulito.Controllers
{
    public class OrdenController : Controller
    {
        private readonly elTriangulitoDBcontex _elTriangulitoDBcontex;

        public OrdenController(elTriangulitoDBcontex elTriangulitoDBcontexto)
        {
            _elTriangulitoDBcontex = elTriangulitoDBcontexto;
        }



        public IActionResult Index(int idMesa)
        {

            DateTime hoy = DateTime.Today;
            int cantidadOrdenesHoyMasUno = _elTriangulitoDBcontex.Cuenta
                                        .Count(c => c.Fecha_Hora.HasValue && c.Fecha_Hora.Value.Date == hoy) + 1;

            // Almacenar el resultado en un ViewData

            ViewData["ListaCuentassCount"] = cantidadOrdenesHoyMasUno;

            var listaEstados = (from e in _elTriangulitoDBcontex.estados
                                where e.tipo_estado == "Orden"
                                select e).ToList();

            ViewData["Estados"] = new SelectList(listaEstados, "id_estado", "nombre");


            var mesa = _elTriangulitoDBcontex.mesas.FirstOrDefault(m => m.id_mesa == idMesa);

            if (mesa == null)
            {
                // Si la mesa no se encuentra en la base de datos.
                return NotFound(); // Devuelve una respuesta HTTP 404 - Not Found
            }

            // Pasar los detalles de la mesa a la vista
            ViewData["IdMesa"] = mesa.id_mesa;
            ViewData["NumeroMesa"] = mesa.nombre_mesa;
            ViewData["CantidadPersonas"] = mesa.cantidad_personas;



            var listadoDetallePedidos = (from dp in _elTriangulitoDBcontex.Detalle_Pedido
                                         join cue in _elTriangulitoDBcontex.Cuenta on dp.Id_cuenta equals cue.Id_cuenta
                                         join me in _elTriangulitoDBcontex.mesas on cue.Id_mesa equals me.id_mesa
                                         join im in _elTriangulitoDBcontex.items_menu on dp.Id_plato equals im.id_item_menu
                                         where cue.Id_mesa == idMesa
                                         select new
                                         {
                                             dp.Cantidad,
                                             Pedido = im.nombre,
                                             dp.Precio,
                                             TipoPlato = dp.Tipo_Plato,
                                             iDCuenta = dp.Id_DetalleCuenta,
                                             idPlato = dp.Id_plato
                                         }).ToList();

            ViewData["listadoDetallePedido"] = listadoDetallePedidos;


            var totalPrecio = (from dp in _elTriangulitoDBcontex.Detalle_Pedido
                               join cue in _elTriangulitoDBcontex.Cuenta on dp.Id_cuenta equals cue.Id_cuenta
                               join me in _elTriangulitoDBcontex.mesas on cue.Id_mesa equals me.id_mesa
                               join im in _elTriangulitoDBcontex.items_menu on dp.Id_plato equals im.id_item_menu
                               where cue.Id_mesa == idMesa
                               select dp.Precio).Sum();
            ViewData["TotalPrecio"] = totalPrecio;




            return View();
        }

        public async Task<IActionResult> ActualizarEstadoMesa(int idMesa, int nuevoIdEstado)
        {
            var mesa = await _elTriangulitoDBcontex.mesas.FindAsync(idMesa);
            if (mesa != null)
            {
                mesa.id_estado = nuevoIdEstado;
                await _elTriangulitoDBcontex.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Mesas");
        }


        public int ObtenerIdEstadoOcupada()
        {
            var estadoOcupada = _elTriangulitoDBcontex.estados.FirstOrDefault(e => e.tipo_estado == "mesas" && e.nombre == "ocupada");
            return estadoOcupada?.id_estado ?? 0; // Devuelve 0 si no se encuentra el estado "Ocupada"
        }




        [HttpPost]
        public async Task<IActionResult> CrearComentario(int idProducto, int idPedido, string Comentario)
        {

            var nuevoComentario = new comentario_pedidos
            {
                Id_producto = idProducto,
                Id_Pedido = idPedido,
                Comentario = Comentario
            };

            _elTriangulitoDBcontex.comentario_pedidos.Add(nuevoComentario);
            await _elTriangulitoDBcontex.SaveChangesAsync();

            return RedirectToAction("Index", "Mesas");
        }


        [HttpPost]
        public async Task<IActionResult> CrearOrden(int idCuenta, int idPlato, int cantidad, string estadoOrden, char tipoPlato, decimal precioO)
        {
            var nuevaOrden = new Detalle_Pedido
            {
                Id_cuenta = idCuenta,
                Id_plato = idPlato,
                Cantidad = cantidad,
                Estado = estadoOrden,
                Tipo_Plato = tipoPlato,
                Precio = precioO
            };

            _elTriangulitoDBcontex.Detalle_Pedido.Add(nuevaOrden);
            await _elTriangulitoDBcontex.SaveChangesAsync();

            return RedirectToAction("Index", "Mesas");
        }

        [HttpPost]
        public async Task<IActionResult> CrearCuenta(int idMesa, string nombre, int cantPersonas, string estadoCuenta, DateTime fechaHora)
        {
            var nuevaCuenta = new Cuenta
            {
                Id_mesa = idMesa,
                Nombre = nombre,
                Cantidad_Personas = cantPersonas,
                Estado_cuenta = estadoCuenta,
                Fecha_Hora = fechaHora
            };

            _elTriangulitoDBcontex.Cuenta.Add(nuevaCuenta);
            await _elTriangulitoDBcontex.SaveChangesAsync();

            // Llama al método para actualizar el estado de la mesa a "Ocupado" (ID = 1)
            await ActualizarEstadoMesa(idMesa, 1);

            // Redirige a la acción "Index" del controlador "DetallesController" con el parámetro "idMesaCuenta"
            return RedirectToAction("Index", "Detalles", new { idMesaCuenta = idMesa });
        }




        public IActionResult AbrirPedidoPorCategoria(int idMesa, int idCategoria)
        {
            // Redirecciona al controlador de pedido y pasa el ID de la categoría como parámetro
            return RedirectToAction("Index", "Pedido", new { idCategoria, idMesa });
        }

    }
}
