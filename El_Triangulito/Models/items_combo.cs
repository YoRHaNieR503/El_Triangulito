using System.ComponentModel.DataAnnotations;

namespace ModuloMeseros.Models
{
    public class items_combo
    {
        [Key]
        public int id_combo {  get; set; }
        public int id_item_menu { get; set; }

    }
}
