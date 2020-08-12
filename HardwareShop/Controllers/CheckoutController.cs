using System;
using System.Collections.Generic;
using System.IO;
using HardwareShop.Helpers;
using HardwareShop.Models;
using Microsoft.AspNetCore.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using System.Net.Mail;
using System.Net;

namespace HardwareShop.Controllers
{
    [Route("Checkout")]
    public class CheckoutController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public CheckoutController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        [Route("checkout")]
        public IActionResult Checkout(int id, string usuario)
        {
            if (id == 0)
            {
                return View("../Account/Login");
            }
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            Factura factura = new Factura();
            factura.Items = cart;
            DateTime FechaActual =
            factura.FechaCompra = DateTime.Now;
            DataContextUsers db = HttpContext.RequestServices.GetService(typeof(DataContextUsers)) as DataContextUsers;
            List<Account> listaUsuarios = db.GetAllAccounts();
            foreach (Account a in listaUsuarios)
            {
                if (a.Usuario == usuario)
                {
                    factura.Account = a;
                }
            }
            DataContextFactura dbf = new DataContextFactura("server=192.168.254.6;port=3306;database=pk;user=admin;password=1111");
            factura.Id = dbf.SetFactura(factura);
            return View("Checkout", model: factura);
        }

        [Route("PDF")]
        public IActionResult PDF()
        {
            int id = 0;

            id = int.Parse(Request.Query["id"].ToString());
            DataContextFactura dbf = new DataContextFactura("server=192.168.254.6;port=3306;database=pk;user=admin;password=1111");
            Factura FacturaActual = dbf.GetFactura(id);

            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"id_" + FacturaActual.Id + "_factura.pdf";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }


            // Creamos el documento con el tamaño de página tradicional
            Document doc = new Document(PageSize.LETTER);
            // Indicamos donde vamos a guardar el documento
            PdfWriter writer = PdfWriter.GetInstance(doc,
                                        new FileStream(sWebRootFolder + "/PDF/" + sFileName, FileMode.Create));



            // Le colocamos el título y el autor
            // **Nota: Esto no será visible en el documento
            doc.AddTitle("Mi primer PDF");
            doc.AddCreator("Nombre de autor");

            // Abrimos el archivo
            doc.Open();



            // Creamos el tipo de Font que vamos utilizar
            iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            // Escribimos el encabezamiento en el documento
            doc.Add(new Paragraph("LA FACTURA ES"));
            doc.Add(Chunk.NEWLINE);

            // Creamos una tabla que contendrá el nombre, apellido y zona
            // de nuestros visitante.
            PdfPTable tblPrueba = new PdfPTable(4);
            tblPrueba.WidthPercentage = 100;

            // Configuramos el título de las columnas de la tabla
            PdfPCell clNombre = new PdfPCell(new Phrase("Nombre y apellidos", _standardFont));
            clNombre.BorderWidth = 0;
            clNombre.BorderWidthBottom = 0.75f;

            PdfPCell clCorreo = new PdfPCell(new Phrase("Correo ", _standardFont));
            clCorreo.BorderWidth = 0;
            clCorreo.BorderWidthBottom = 0.75f;

            PdfPCell clVentas = new PdfPCell(new Phrase("Productos comprados ", _standardFont));
            clVentas.BorderWidth = 0;
            clVentas.BorderWidthBottom = 0.75f;

            PdfPCell clCantidad = new PdfPCell(new Phrase("Cantidad", _standardFont));
            clCantidad.BorderWidth = 0;
            clCantidad.BorderWidthBottom = 0.75f;

            // Añadimos las celdas a la tabla
            tblPrueba.AddCell(clNombre);
            tblPrueba.AddCell(clCorreo);
            tblPrueba.AddCell(clVentas);
            tblPrueba.AddCell(clCantidad);


            // Cambiamos el tipo de Font para el listado
            _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLUE);
            // Llenamos la tabla con el primer cliente
            clNombre = new PdfPCell(new Phrase(FacturaActual.Account.Nombre, _standardFont));
            clNombre.BorderWidth = 0;
            tblPrueba.AddCell(clNombre);
            clCorreo = new PdfPCell(new Phrase(FacturaActual.Account.Correo, _standardFont));
            clCorreo.BorderWidth = 0;
            tblPrueba.AddCell(clCorreo);

            foreach (Item i in FacturaActual.Items)
            {
                clNombre = new PdfPCell(new Phrase(""));
                clNombre.BorderWidth = 0;
                tblPrueba.AddCell(clNombre);
                clCorreo = new PdfPCell(new Phrase(""));
                clCorreo.BorderWidth = 0;
                tblPrueba.AddCell(clCorreo);
                clVentas = new PdfPCell(new Phrase(i.Product.Nombre, _standardFont));
                clVentas.BorderWidth = 0;
                tblPrueba.AddCell(clVentas);
                clCantidad = new PdfPCell(new Phrase(i.Quantity.ToString(), _standardFont));
                clCantidad.BorderWidth = 0;
                tblPrueba.AddCell(clCantidad);
            }
            doc.Add(tblPrueba);
            //// Llenamos la tabla con el segundo cliente
            //clNombre = new PdfPCell(new Phrase("María", _standardFont));
            //clNombre.BorderWidth = 0;
            //clApellido = new PdfPCell(new Phrase("Sánchez", _standardFont));
            //clApellido.BorderWidth = 0;
            //clZona = new PdfPCell(new Phrase("SUR", _standardFont));
            //clZona.BorderWidth = 0;
            //clVentas = new PdfPCell(new Phrase("321.05", _standardFont));
            //clZona.BorderWidth = 0;
            //// Añadimos las celdas a la tabla
            //tblPrueba.AddCell(clNombre);
            //tblPrueba.AddCell(clApellido);
            //tblPrueba.AddCell(clZona);
            //tblPrueba.AddCell(clVentas);

            // Finalmente, añadimos la tabla al documento PDF y cerramos el documento

            doc.Close();
            writer.Close();

            ViewBag.URL = URL;

            // servidor SMTP

            SmtpClient client = new SmtpClient("smtp.gmail.com");
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("hiberusclaseaspcoremvc@gmail.com", "111??aaa");
            client.EnableSsl = true;

            // 
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("hiberusclaseaspcoremvc@gmail.com");
            mailMessage.To.Add(FacturaActual.Account.Correo);
            mailMessage.Body = "Para poder ver este pdf acude a esta página web"+URL;
            mailMessage.Subject = "Factura de la compra";

            string output = "enviado";
            try
            {
                client.Send(mailMessage);
            }
            catch (Exception e) { output = e.ToString() + "no enviado"; }
            return View("../Finito/Index",model:FacturaActual);
        }
    }
}