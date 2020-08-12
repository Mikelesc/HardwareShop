using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace HardwareShop.Models
{
    public class DataContextFactura
    {
        public string ConnectionString { get; set; }

        public DataContextFactura(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public int SetFactura(Factura nuevaFactura)
        {
            int i = 0;
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd1 = new MySqlCommand(String.Format("INSERT INTO facturas(`FechaFactura`,`IdUsuario`) VALUES ('{0}','{1}');", nuevaFactura.FechaCompra.ToString("yyyy-MM-dd HH:mm:ss"), nuevaFactura.Account.Id), conn);       
                cmd1.ExecuteNonQuery();
                foreach (Item item in nuevaFactura.Items)
                {
                    MySqlCommand cmd2 = new MySqlCommand(String.Format("INSERT INTO lineafacturas(`Id_factura`,`Id_Producto`,`Cantididad`) VALUES ((SELECT max(id) FROM facturas),'{0}','{1}');", item.Product.Id, item.Quantity), conn);
                    cmd2.ExecuteNonQuery();
                }
                i = (int) cmd1.LastInsertedId;
            }
            return i;
        }

        public Factura GetFactura(int id)
        {
            Factura Factura_Actual = new Factura();
            DataContextProducts dbp = new DataContextProducts("server=192.168.254.6;port=3306;database=pk;user=admin;password=1111");
            List<Product> listaProductos = dbp.GetAllProducts();
            DataContextUsers dbu = new DataContextUsers("server=192.168.254.6;port=3306;database=pk;user=admin;password=1111");
            List<Account> listaUsuarios = dbu.GetAllAccounts();
            List<Item> Items = new List<Item>();
            int Id_Factura = 0;
            int Id1 = 0;
            int Id_Producto = 0;
            int Cantididad = 0;
            int Id2 = 0;
            int Id_Usuario = 0;
            DateTime FechaFactura = new DateTime();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd1 = new MySqlCommand(String.Format("SELECT * FROM lineafacturas WHERE Id_Factura = {0};", id), conn);
                using (MySqlDataReader reader = cmd1.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Id1 = reader.GetInt32("Id");
                        Id_Factura = reader.GetInt32("Id_Factura");
                        Id_Producto = reader.GetInt32("Id_Producto");
                        Cantididad = reader.GetInt32("Cantididad");
                        foreach (Product p in listaProductos)
                        {
                            if (p.Id == Id_Producto)
                            {
                                Items.Add(new Item(p, Cantididad));
                            }
                        }
                    }
                }
                MySqlCommand cmd2 = new MySqlCommand(String.Format("SELECT * FROM facturas WHERE Id = {0};", Id_Factura), conn);
                using (MySqlDataReader reader = cmd2.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Id2 = reader.GetInt32("id");
                        FechaFactura = reader.GetDateTime("FechaFactura");
                        Id_Usuario = reader.GetInt32("IdUsuario");
                    }
                }
                foreach(Account a in listaUsuarios)
                {
                    if (Id_Usuario == a.Id)
                    {
                        Factura_Actual.Account = a; // USUARIO QUE EJECUTA LA FACTURA
                    }
                }
            }
            Factura_Actual.Items = Items; // ITEMS QUE CONFORMAN LA FACUTA
            Factura_Actual.FechaCompra = FechaFactura; // FECHA DE LA FACTURA
            Factura_Actual.Id = id;
            return Factura_Actual;
        }
    }
}
