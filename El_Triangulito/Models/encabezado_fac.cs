using System.ComponentModel.DataAnnotations;

namespace El_Triangulito.Models
{
    public class encabezado_fac
    {

        [Key]
        public int id_factura { get; set; }
        public int? id_pedido { get; set; }
        public int? iid_cliented { get; set; }
        public DateTime? fecha_cobro { get; set; }
        public decimal? total_cobrado { get; set; }
        public string? metodo_pago { get; set; }


    }
}
