using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ass1.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string CartId { get; set; }

        public int Item { get; set; }
        public string NameOfItem { get; set; }
        public decimal Price { get; set; }
        public string Seller { get; set; }
        public int Quantity { get; set; }
    }
}
