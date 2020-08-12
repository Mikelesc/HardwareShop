using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HardwareShop.Models
{
    public class Item
    {
        public Item() { }
        public Item(Product p, int Cantidad)
        {
            this.Product = p;
            this.Quantity = Cantidad;
        }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
