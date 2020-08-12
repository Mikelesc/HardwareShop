using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HardwareShop.Models
{
    public class Factura
    {
        public List<Item> Items { get; set; }
        public DateTime FechaCompra { get; set; }
        public Account Account { get; set; }
        public int Id { get; set; }
    }
}