using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HardwareShop.Helpers;
using HardwareShop.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HardwareShop.Controllers
{
    [Route("wish")]
    public class WishController : Controller
    {
        [Route("index")]
        public IActionResult Index()
        {
            var wish = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "wish");
            if (wish == null)
            {
                return View("../Wish/Empty");
            }
            else
            {
                return View(wish);
            }
        }
       

        [Route("buy/{id}")]
        public IActionResult Buy(int id)
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
                int index = isExist(id);
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
                int index = isExist2(id);
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


        [Route("remove/{id}")]
        public IActionResult Remove(int id)
        {
            List<Item> wish = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "wish");
            int index = isExist(id);
            wish.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "wish", wish);
            return RedirectToAction("Index");
        }

        [Route("removeall")]
        public IActionResult RemoveAll()
        {
            List<Item> wish = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "wish");
            foreach (Item i in wish)
            {
                wish.Remove(i);
            }
            SessionHelper.SetObjectAsJson(HttpContext.Session, "wish", wish);
            return RedirectToAction("Index");
        }


        private int isExist(int id)
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


        //METODO AUXILIARRRRRRRR
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
