using Microsoft.AspNetCore.Mvc;
using ModuloMeseros.Models;

namespace ModuloMeseros.Controllers
{
    public class MesasController : Controller
    {

        private readonly elTriangulitoDBcontex _DulceSavorDbContext;

        public MesasController(elTriangulitoDBcontex DulceSavorDbContexto)
        {
            _DulceSavorDbContext = DulceSavorDbContexto;
        }


        public IActionResult Index()
        {

            int cantidadMesasOcupadas = (from m in _DulceSavorDbContext.mesas
                                       join em in _DulceSavorDbContext.estados on m.id_estado equals em.id_estado
                                       where em.tipo_estado == "mesas"
                                       && em.nombre == "ocupada"
                                       select m
                                       ).Count();

            ViewData["ListaMesasOcupadas"] = cantidadMesasOcupadas;

            int cantidadMesas = (from m in _DulceSavorDbContext.mesas
                                       select m
                                       ).Count();

            ViewData["ListaMesasCount"]=cantidadMesas;


            var listadoMesasTodas = (from m in _DulceSavorDbContext.mesas
                                    join em in _DulceSavorDbContext.estados on m.id_estado equals em.id_estado
                                     where em.tipo_estado == "mesas"
                                     select new
                                    {
                                        IDMesa=m.id_mesa,
                                        Numero_Mesa = m.nombre_mesa,
                                        CantidasPersonas = m.cantidad_personas,
                                        Estado = em.nombre

                                    }).ToList();
            ViewData["listadoMesaTodas"] = listadoMesasTodas;


            return View();
        }
    }
}
