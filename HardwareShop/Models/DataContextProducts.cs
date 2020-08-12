using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HardwareShop.Models
{
    public class DataContextProducts
    {

        public string ConnectionString { get; set; }

        public DataContextProducts(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }


        public List<Product> GetAllProducts()
        {
            List<Product> list = new List<Product>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM productos", conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Product()
                        {
                            Id = reader.GetInt32("id"),
                            Nombre = reader.GetString("nombre"),
                            Precio = reader.GetDouble("precio"),
                            Tipo = reader.GetString("tipo"),                          
                            Imagen=reader.GetString("imagen"),
                            Descripcion=reader.GetString("descripcion")
                        });
                    }
                }
            }
            return list;
        }
        public Product find(int id,List<Product> listaProductos)
        {
            return listaProductos.Single(p => p.Id.Equals(id));
        }
    }
}
