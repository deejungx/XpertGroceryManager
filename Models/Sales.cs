using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public virtual ICollection<SalesLineItem> LineItems { get; set; }

        [NotMapped]
        public decimal Total => GetTotal(LineItems);

        private static decimal GetTotal(ICollection<SalesLineItem> LineItems)
        {
            var total = 0.00m;
            //if (LineItems != null)
            //{
            //    foreach (SalesLineItem item in LineItems) {
            //        var lineTotal = item.Quantity * item.Product.Price;
            //        total += lineTotal;
            //    }
            //}
            return total;
        }
    }
}