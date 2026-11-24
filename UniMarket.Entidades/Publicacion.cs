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
        public int ProductoId { get; set; }
        public Producto Producto { get; set; } = new Producto();

        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public bool Activo { get; set; } = true;
        public ModalidadPublicacion Modalidad { get; set; } = ModalidadPublicacion.Venta;
        public string TruequeBusca { get; set; }
        public DateTime FechaPublicacion { get; set; } = DateTime.Now;

        // Propiedades puente: mantienen compatibilidad con tus Forms actuales sin romper nada
        public int VendedorId
        {
            get => Producto?.PropietarioId ?? 0;
            set { if (Producto == null) Producto = new Producto(); Producto.PropietarioId = value; }
        }

        public string Titulo
        {
            get => Producto?.Titulo;
            set { if (Producto == null) Producto = new Producto(); Producto.Titulo = value; }
        }

        public string Descripcion
        {
            get => Producto?.Descripcion;
            set { if (Producto == null) Producto = new Producto(); Producto.Descripcion = value; }
        }

        public byte[] Imagen
        {
            get => Producto?.Imagen;
            set { if (Producto == null) Producto = new Producto(); Producto.Imagen = value; }
        }

        // Tus métodos de dominio originales intactos
        public bool AceptaVenta => Modalidad != ModalidadPublicacion.Trueque;
        public bool AceptaTrueque => Modalidad != ModalidadPublicacion.Venta;

        public bool PerteneceA(int usuarioId) => VendedorId == usuarioId;
        public bool TieneStockPara(int cantidad) => Stock >= cantidad;
    }
}