using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HardwareShop.Helpers;
using HardwareShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace HardwareShop.Controllers
{
    [Route("componente")]
    public class ComponenteController : Controller
    {
        [Route("index")]
        public IActionResult Index()
        {
            DataContextProducts db = HttpContext.RequestServices.GetService(typeof(DataContextProducts)) as DataContextProducts;
            List<Product> listaProductos = db.GetAllProducts();
            List<Product> componentes = new List<Product>();
            foreach (Product p in listaProductos)
            {
                if (p.Tipo.Contains("componente"))
                {                   
                    componentes.Add(p);
                    //COMPONENTES ES UNA LISTA DE PRODUCTOS Y TU LA TRATAS COMO LISTA DE ITEMS MELON
                }
            }
            return View(model: componentes);
        }

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

        [Route("añadir2/{id}")]
        public IActionResult Añadir2(int id)
        {
            DataContextProducts db = HttpContext.RequestServices.GetService(typeof(DataContextProducts)) as DataContextProducts;
            List<Product> listaProductos = db.GetAllProducts();
            if (SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "wish") == null)
            {
                List<Item> wish = new List<Item>();
                wish.Add(new Item { Product = db.find(id, listaProductos), Quantity = 1 });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "wish", wish);
            }
            else
            {
                List<Item> wish = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "wish");
                int index = isExist2(id);
                if (index != -1)
                {
                    wish[index].Quantity++;
                }
                else
                {
                    wish.Add(new Item { Product = db.find(id, listaProductos), Quantity = 1 });
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "wish", wish);
            }
            return RedirectToAction("Index");
        }

        /* [Route("mostrar")]
         public IActionResult mostrar(int id)
         {
             DataContextProducts db = HttpContext.RequestServices.GetService(typeof(DataContextProducts)) as DataContextProducts;
             Product product2 = new Product();
             List<Product> listaProductos = db.GetAllProducts();           
             foreach (Product p in listaProductos)
             {
                 if (p.Id.Equals(id))
                 {
                     product2 = p;
                     break;
                     //COMPONENTES ES UNA LISTA DE PRODUCTOS Y TU LA TRATAS COMO LISTA DE ITEMS MELON
                 }
             }

             ViewBag.mostrarimagen = product2.Imagen;
             ViewBag.mostrarid = product2.Id;

             return View("Index");
         }
         */


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
        private int isExist2(int id)
        {
            List<Item> wish = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "wish");
            for (int i = 0; i < wish.Count; i++)
            {
                if (wish[i].Product.Id.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }

    }
}