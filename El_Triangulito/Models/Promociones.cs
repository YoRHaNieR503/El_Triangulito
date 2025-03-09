using System.ComponentModel.DataAnnotations;

namespace El_Triangulito.Models
{
    public class Promociones
    {

        [Key]
        public int id_promo { get; set; }
        public string? descripcion { get; set; }
        public decimal? precio { get; set; }
        public DateOnly? fecha_inicio { get; set; }
        public DateOnly? fecha_final { get; set; }
        public string? imagen { get; set; }
        public int? id_estado { get; set; }
        public string? nombre { get; set; }


    }
}
