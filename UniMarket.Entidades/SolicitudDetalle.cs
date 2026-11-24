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
        public EstadoSolicitud Estado { get; set; }
        public DateTime Fecha { get; set; }
        public string NombreContraparte { get; set; }
        public string TelefonoContacto { get; set; }

        // Nuevas propiedades de la tabla 'trueques'
        public bool ConfirmadoSolicitante { get; set; }
        public bool ConfirmadoDueno { get; set; }
        public string EstadoEntrega { get; set; }

        // Propiedades de apoyo para la interfaz
        public decimal Total => Cantidad * PrecioUnitario;
        public bool EsTrueque => Tipo == TipoSolicitud.Trueque;
        public bool EstaPendiente => Estado == EstadoSolicitud.Pendiente;
        public bool EstaAceptada => Estado == EstadoSolicitud.Aceptada;
        public bool TruequeCompletado => EstadoEntrega == "Completado";
    }
}