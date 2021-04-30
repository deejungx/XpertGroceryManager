using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace XpertGroceryManager.Models.ViewModels
{
    public class SalesViewModel
    {
        public int SalesId {get; set;}

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date {get; set;}
        public int CustomerId {get; set;}
        public string CustomerName {get; set;}
        public IEnumerable<LineItemViewModel> LineItems {get; set;}

        [NotMapped]
        public decimal SalesTotal => LineItems.Sum(i => i.Quantity * i.UnitCost);
    }

    public class LineItemViewModel
    {
        public int ProductId {get; set;}
        public string ProductName {get; set;}
        public decimal UnitCost {get; set;}
        public int Quantity {get; set;}
    }
}
