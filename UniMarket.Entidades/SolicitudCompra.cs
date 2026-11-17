using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniMarket.Entidades
{
    public class SolicitudCompra
    {

        public int Id { get; set; }
        public int PublicacionId { get; set; }
        public int CompradorId { get; set; }
        public int Cantidad { get; set; }
        public string Estado { get; set; }
        public DateTime Fecha { get; set; }
    }
}
