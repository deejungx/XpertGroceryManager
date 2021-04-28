using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace XpertGroceryManager.Models
{
    public class Sales
    {
        [Key]
        public int Id { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Sales date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime SalesDate { get; set; }

        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public virtual ICollection<PurchaseLineItem> LineItems { get; set; } 
    }
}