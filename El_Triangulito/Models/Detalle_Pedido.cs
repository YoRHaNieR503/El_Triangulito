using System.ComponentModel.DataAnnotations;

namespace ModuloMeseros.Models
{
    public class Detalle_Pedido
    {

        [Key]
        public int Id_DetalleCuenta { get; set;}
        public int? Id_cuenta { get; set;}
        public int? Id_plato { get; set;}
        public int? Cantidad { get; set;}
        public string Estado { get; set;}
        public char Tipo_Plato { get; set;}
        public decimal? Precio { get; set;}
    } 
}
