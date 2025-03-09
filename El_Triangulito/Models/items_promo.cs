using System.ComponentModel.DataAnnotations;

namespace El_Triangulito.Models
{
    public class items_promo
    {
        [Key]
        public int? id_promo { get; set; }
        public int? id_item_menu { get; set; }
    }
}
