using System.ComponentModel.DataAnnotations;

namespace ModuloMeseros.Models
{
    public class Cuenta
    {
        [Key]
        public int Id_cuenta { get; set; }
        public int Id_mesa {  get; set; }
        public string? Nombre { get; set; }
        public int? Cantidad_Personas {  get; set; }
        public string? Estado_cuenta { get; set; }
        public DateTime? Fecha_Hora { get; set; }
    }
}
