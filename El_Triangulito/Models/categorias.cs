using System.ComponentModel.DataAnnotations;

namespace El_Triangulito.Models
{
    public class categorias
    {

        [Key]
        public int id_categoria { get; set; }

        [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
        public string? categoria { get; set; }

    }
}
