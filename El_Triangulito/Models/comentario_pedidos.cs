using System.ComponentModel.DataAnnotations;

namespace ModuloMeseros.Models
{
    public class comentario_pedidos
    {

        [Key]
        public int Id_comentario_Pedido { get; set; }
        public int? Id_producto { get; set; }
        public int? Id_Pedido { get; set; }
        public string? Comentario { get; set; }

    }
}
