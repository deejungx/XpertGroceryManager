using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XpertGroceryManager.Models
{
    public class Stock
    {
        [Key]
        [ForeignKey("Product")]
        public int Id { get; set; }

        [Required]
        [Range(0, 999)]
        public int Quantity { get; set; }

        public virtual Product Product { get; set; }
    }
}