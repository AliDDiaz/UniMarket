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
        public TipoSolicitud Tipo { get; set; }
        public string OfertaTrueque { get; set; }
        public bool EsTrueque => Tipo == TipoSolicitud.Trueque;
        public EstadoSolicitud Estado { get; set; }
        public DateTime Fecha { get; set; }
        public string NombreContraparte { get; set; }
        public string TelefonoContacto { get; set; }

        public decimal Total => Cantidad * PrecioUnitario;
        public bool EstaPendiente => Estado == EstadoSolicitud.Pendiente;
    }
}