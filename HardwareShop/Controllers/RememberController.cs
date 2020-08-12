using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using HardwareShop.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HardwareShop.Controllers
{   
    
        [Route("index")]
        public class RememberController : Controller
        {
            [Route("index")]
            public IActionResult Index()
            {
                return View();
            }
            [Route("remember")]
            [HttpPost]
            public IActionResult Remember(string correo)
            {
                // Leer de base de datos
                DataContextUsers db = HttpContext.RequestServices.GetService(typeof(DataContextUsers)) as DataContextUsers;
                List<Account> listaUsuarios = db.GetAllAccounts();

                // servidor SMTP

                SmtpClient client = new SmtpClient("smtp.gmail.com");
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("hiberusclaseaspcoremvc@gmail.com", "111??aaa");
                client.EnableSsl = true;

                // 
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("hiberusclaseaspcoremvc@gmail.com");
                foreach (Account a in listaUsuarios)
                {
                    if (a.Correo == correo)
                    {
                        mailMessage.To.Add(correo);
                        mailMessage.Body = "La contraseña que tenías era\n\n" + a.Contraseña + "\n\na ver si no la vuelves a olvidar melón";
                    }
                }
                mailMessage.Subject = "Olvidación de la contraseña";
                string output = "enviado";
                try
                {
                    client.Send(mailMessage);
                }
                catch (Exception e) { output = e.ToString() + "no enviado"; }

                ViewBag.message = output;
                return View("Success");
            }
        }
   
}
