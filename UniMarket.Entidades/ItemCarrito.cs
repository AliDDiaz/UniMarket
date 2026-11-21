using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniMarket.Entidades
{
    public class ItemCarrito
    {
        public Publicacion Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal => Producto.Precio * Cantidad;
    }
}