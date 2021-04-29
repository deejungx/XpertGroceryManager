using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XpertGroceryManager.Models.ViewModels
{
    public class LineItemProductData
    {
        public int ProductId {get; set;}
        public string Name { get; set; }
        public bool Selected { get; set; }
        public int Quantity { get; set; }
    }
}
