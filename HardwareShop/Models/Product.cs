using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HardwareShop.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public double Precio { get; set; }
        public string Tipo { get; set; }
        public string Imagen { get; set; }
        public string Descripcion { get; set; }
    }
}
