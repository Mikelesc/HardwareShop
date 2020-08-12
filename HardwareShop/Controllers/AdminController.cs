using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HardwareShop.Controllers
{
    [Route("admin")]
    public class AdminController : Controller
    {
        [Route("index")]
        public IActionResult Index(int admin)
        {
            if (admin == 0)
            {
                return View("NoAdmin");
            }
            else
            {
                return View("Index");
            }
           
        }
    }
}