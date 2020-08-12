using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HardwareShop.Models
{
    public class DataContextUsers
    {

        public string ConnectionString { get; set; }

        public DataContextUsers(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }


        public List<Account> GetAllAccounts()
        {
            List<Account> list = new List<Account>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM usuarios", conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int Id = reader.GetInt32("id");
                        string Usuario = reader.GetString("usuario");
                        string Contraseña = reader.GetString("contraseña");
                        string Nombre = reader.GetString("nombre");
                        string Correo = reader.GetString("correo");
                        int Activado = reader.GetInt32("activado");
                        int Random = reader.GetInt32("random");
                        string Foto = reader.GetString("Foto");
                        int Administrador = reader.GetInt32("Administrador");

                        list.Add(new Account(Nombre, Usuario, Id, Contraseña, Correo, Activado, Random,Foto,Administrador));
                    }
                }
            }
            return list;
        }
        public void SetAccount(Account nuevaCuenta)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(String.Format("INSERT INTO usuarios(`Usuario`,`Nombre`,`Correo`,`Contraseña`,`Activado`,`Random`,`Administrador`) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}');",nuevaCuenta.Usuario,nuevaCuenta.Nombre,nuevaCuenta.Correo,nuevaCuenta.Contraseña,nuevaCuenta.Activado,nuevaCuenta.Random,nuevaCuenta.Administrador), conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                    }
                    conn.Close();
                }
            }
        }
        
    }
}
