using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using HardwareShop.Helpers;
using HardwareShop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HardwareShop.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        [Route("login")]
        [HttpGet]
        public IActionResult Login(int id, string usuario)
        {
            DataContextUsers db = HttpContext.RequestServices.GetService(typeof(DataContextUsers)) as DataContextUsers;
            List<Account> listaUsuarios = db.GetAllAccounts();
            Account perfil = new Account();
            foreach(Account a in listaUsuarios)
             {
                if (usuario == a.Usuario)
                {
                    perfil = a;
                }
            }

            if (id == 0)
            {
                return View("../Account/Login");
            }
            return View("Logged", model: perfil);
        }

        [Route("remember")]
        [HttpGet]
        public IActionResult remember()
        {
            return View("remember");
        }

        [Route("login")]
        [HttpPost]
        public IActionResult Login(string usuario, string contraseña)
        {
            DataContextUsers db = HttpContext.RequestServices.GetService(typeof(DataContextUsers)) as DataContextUsers;
            List<Account> listaUsuarios = db.GetAllAccounts();
            int SessionStatus = -1;

            foreach(Account a in listaUsuarios)
            {
                if ((usuario == a.Usuario)&&(contraseña == a.Contraseña))
                {
                    HttpContext.Session.SetString("usuario", usuario);
                    SessionStatus = 1;
                    HttpContext.Session.SetInt32("logged", SessionStatus);
                    HttpContext.Session.SetInt32("administrador", a.Administrador);
                    DataContextProducts db2 = HttpContext.RequestServices.GetService(typeof(DataContextProducts)) as DataContextProducts;
                    List<Product> listaProductos = db2.GetAllProducts();
                    return View("../Home/Index", model: listaProductos);                    
                }
            }
            ViewBag.error = "Usuario o contraseña incorrectos";                     
            return View("Login");           
        }


        [Route("logout")]
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("username");
            int SessionStatus = 0;
            HttpContext.Session.SetInt32("logged", SessionStatus);
            return RedirectToAction("login");
        }

        [Route("register")]
        [HttpGet]
        public IActionResult Add()
        {
            return View("register");
            
        }

        [Route("register")]
        [HttpPost]
        public IActionResult Add(string nombre, string correo, string usuario, string contraseña, string foto)
        {
            DataContextUsers db = HttpContext.RequestServices.GetService(typeof(DataContextUsers)) as DataContextUsers;
            List<Account> listaUsuarios = db.GetAllAccounts();
            int activado = 0;
            Random rnd = new Random();
            int random = rnd.Next(0, 9999999);

            int Administrador = 0;
            Account nuevaCuenta = new Account(nombre, usuario, contraseña, correo, activado, random, foto,Administrador);

            foreach (Account a in listaUsuarios)
            {
                if ((nuevaCuenta.Usuario == a.Usuario))
                {
                    ViewBag.error = "Ya existe ese nombre de usuario";
                    return View("register");
                }
            }
            // servidor SMTP

            //SmtpClient client = new SmtpClient("smtp.gmail.com");
            //client.UseDefaultCredentials = false;
            //client.Credentials = new NetworkCredential("hiberusclaseaspcoremvc@gmail.com", "111??aaa");
            //client.EnableSsl = true;

            //// 
            //MailMessage mailMessage = new MailMessage();
            //mailMessage.From = new MailAddress("hiberusclaseaspcoremvc@gmail.com");
            //mailMessage.To.Add(nuevaCuenta.Correo);
            //mailMessage.Body = "Hola, bienvenido a la mejor página web de la historia de España y parte de Norte América. Haz click en el siguiente enlace para verificar tu usuario" +
            //    "<a href="http://localhost:puerto/account/idUsuario?=25&ticket=123456""
            db.SetAccount(nuevaCuenta);
            return RedirectToAction("verify");
        }

        [Route("verify")]
        [HttpGet]
        public IActionResult Verify()
        {
            return View("login");
        }

        [Route("remember")]
        [HttpPost]
        public IActionResult Remember(string usuario)
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
            foreach(Account a in listaUsuarios)
            {
                if (a.Usuario == usuario)
                {
                    mailMessage.To.Add(a.Correo);
                    mailMessage.Body = "La contraseña que tenías era" + a.Contraseña + "a ver si no la vuelves a olvidar melón";
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