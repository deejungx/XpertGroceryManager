using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XpertGroceryManager.Models
{
    public class SalesLineItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(0, 999)]
        public int Quantity { get; set; }

        public int? SalesId { get; set; }
        public virtual Sales Sales { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
