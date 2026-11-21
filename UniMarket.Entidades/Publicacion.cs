using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniMarket.Entidades
{
    public class Publicacion
    {

        public int Id { get; set; }
        public int VendedorId { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public bool Activo { get; set; }
        public byte[] Imagen { get; set; }

        public bool PerteneceA(int usuarioId) => VendedorId == usuarioId;

        public bool TieneStockPara(int cantidad) => Stock >= cantidad;
    }
}
