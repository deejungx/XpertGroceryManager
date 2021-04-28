using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace XpertGroceryManager.Models
{
    public class Purchase
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Vendor { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Purchase date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PurchaseDate { get; set; }

        public virtual ICollection<PurchaseLineItem> LineItems { get; set; } 
    }
}