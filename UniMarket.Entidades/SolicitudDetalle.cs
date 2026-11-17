using System;

namespace UniMarket.Entidades
{
    public class SolicitudDetalle
    {
        public int Id { get; set; }
        public int PublicacionId { get; set; }
        public string Titulo { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public string Estado { get; set; }
        public DateTime Fecha { get; set; }
        public string NombreContraparte { get; set; }
        public string TelefonoContacto { get; set; }

        public decimal Total => Cantidad * PrecioUnitario;
    }
}