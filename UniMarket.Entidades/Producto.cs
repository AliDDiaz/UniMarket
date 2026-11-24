using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace UniMarket.Entidades
{
    public class Producto
    {
        public int Id { get; set; }
        public int PropietarioId { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public byte[] Imagen { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public bool PerteneceA(int usuarioId) => PropietarioId == usuarioId;
    }
}