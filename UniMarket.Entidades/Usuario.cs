using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace UniMarket.Entidades
{
    public class Usuario
    {
        // Tabla: public.usuarios
        public int Id { get; set; }
        public string Correo { get; set; }
        public string Contrasena { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime FechaRegistro { get; set; }

        // Tabla: public.perfiles_usuarios (relación 1:1)
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public byte[] Foto { get; set; }
    }
}