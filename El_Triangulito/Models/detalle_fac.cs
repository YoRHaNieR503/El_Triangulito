using System.ComponentModel.DataAnnotations;

namespace El_Triangulito.Models
{
    public class detalle_fac
    {
        [Key]
        public int id_detallefac { get; set; }
        public int? id_factura { get; set; }
        public int? id_detallepedido { get; set; }
        public decimal? precio { get; set; }
        public decimal? valor { get; set; }
        public decimal? total_plato { get; set; }
        public int? cantidad { get; set; }

    }
}
