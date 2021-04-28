using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XpertGroceryManager.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(120, ErrorMessage = "Product name cannot be longer than 120 characters.")]
        public string Name { get; set; }

        [StringLength(225, ErrorMessage = "Product description cannot be longer than 225 characters.")]
        public string Description { get; set; }

        [Required]
        [Range(0, 99999.99)]
        [Column(TypeName = "decimal(18,4)")]
        [Display(Name = "Unit price")]
        public decimal Price { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public virtual Stock Stock { get; set; }

        public virtual ICollection<SalesLineItem> SalesLineItems { get; set; }

        public virtual ICollection<PurchaseLineItem> PurchaseLineItems { get; set; } 
    }
}