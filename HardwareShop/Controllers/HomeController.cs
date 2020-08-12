using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HardwareShop.Models;
using HardwareShop.Helpers;

namespace HardwareShop.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            DataContextProducts db = HttpContext.RequestServices.GetService(typeof(DataContextProducts)) as DataContextProducts;
            List<Product> listaProductos = db.GetAllProducts();
            return View("Index",model: listaProductos);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        ////////////////////////////////////////AVERLO///////////////////////////
      

        [Route("añadir/{id}")]
        public IActionResult Añadir(int id)
        {
            DataContextProducts db = HttpContext.RequestServices.GetService(typeof(DataContextProducts)) as DataContextProducts;
            List<Product> listaProductos = db.GetAllProducts();
            if (SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart") == null)
            {
                List<Item> cart = new List<Item>();
                cart.Add(new Item { Product = db.find(id, listaProductos), Quantity = 1 });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
                int index = isExist(id);
                if (index != -1)
                {
                    cart[index].Quantity++;
                }
                else
                {
                    cart.Add(new Item { Product = db.find(id, listaProductos), Quantity = 1 });
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            return RedirectToAction("Index");
        }
        // Metodo auxiliarisimo
        private int isExist(int id)
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.Id.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }





    }
}
