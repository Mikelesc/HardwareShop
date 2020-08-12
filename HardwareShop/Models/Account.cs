using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HardwareShop.Models
{
    public class Account
    {
        public Account(string nombre, string usuario, int id, string contraseña, string correo, int activado, int random,string foto,int administrador)

        {
            Nombre = nombre;
            Usuario = usuario;
            Id = id;
            Contraseña = contraseña;
            Correo = correo;
            Activado = activado;
            Random = random;
            Foto = foto;
            Administrador=administrador;

        }
        public Account(string nombre, string usuario, string contraseña, string correo, int activado, int random, string foto,int administrador)
        {
            Nombre = nombre;
            Usuario = usuario;
            Contraseña = contraseña;
            Correo = correo;
            Activado = activado;
            Random = random;
            Foto = foto;
            Administrador = administrador;
        }
        public Account() { }

        [Required]
        public string Nombre { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Usuario { get; set; }
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        [RegularExpression("((?=.*\\d)(?=.*[a-z])(?=.*[A-Z])")]
        public string Contraseña { get; set; }
        [Required]
        public string Correo { get; set; }
        public int Activado { get; set; }
        public int Random { get; set; }
        public string Foto { get; set; }
        public int Administrador { get; set; }
    }
}
