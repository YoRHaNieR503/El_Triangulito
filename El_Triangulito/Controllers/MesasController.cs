using El_Triangulito.Models;
using Microsoft.AspNetCore.Mvc;

namespace El_Triangulito.Controllers
{
    public class MesasController : Controller
    {

        private readonly elTriangulitoDBcontex _elTriangulitoDBcontex;

        public MesasController(elTriangulitoDBcontex elTriangulitoDBcontexto)
        {
            _elTriangulitoDBcontex = elTriangulitoDBcontexto;
        }


        public IActionResult Index()
        {

            int cantidadMesasOcupadas = (from m in _elTriangulitoDBcontex.mesas
                                         join em in _elTriangulitoDBcontex.estados on m.id_estado equals em.id_estado
                                         where em.tipo_estado == "mesas"
                                         && em.nombre == "ocupada"
                                         select m
                                       ).Count();

            ViewData["ListaMesasOcupadas"] = cantidadMesasOcupadas;

            int cantidadMesas = (from m in _elTriangulitoDBcontex.mesas
                                 select m
                                       ).Count();

            ViewData["ListaMesasCount"] = cantidadMesas;


            var listadoMesasTodas = (from m in _elTriangulitoDBcontex.mesas
                                     join em in _elTriangulitoDBcontex.estados on m.id_estado equals em.id_estado
                                     where em.tipo_estado == "mesas"
                                     select new
                                     {
                                         IDMesa = m.id_mesa,
                                         Numero_Mesa = m.nombre_mesa,
                                         CantidasPersonas = m.cantidad_personas,
                                         Estado = em.nombre

                                     }).ToList();
            ViewData["listadoMesaTodas"] = listadoMesasTodas;


            return View();
        }
    }
}
