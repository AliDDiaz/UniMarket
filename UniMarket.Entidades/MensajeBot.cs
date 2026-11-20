using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniMarket.Entidades
{
    public class MensajeBot
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Titulo { get; set; }
        public string Mensaje { get; set; }
        public string Tipo { get; set; } // 'OFERTA_RECIBIDA', 'OFERTA_ACEPTADA', 'OFERTA_RECHAZADA'
        public int? SolicitudId { get; set; }
        public bool Leido { get; set; }
        public DateTime Fecha { get; set; }
    }
}
