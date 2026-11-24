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
        public int SolicitanteId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public TipoSolicitud Tipo { get; set; } = TipoSolicitud.Compra;
        public EstadoSolicitud Estado { get; set; }
        public DateTime Fecha { get; set; }

        // Relación 1 a 1 con la nueva tabla 'trueques'
        public AcuerdoTrueque Trueque { get; set; } = new AcuerdoTrueque();

        // Propiedades puente: mantienen 100% compatibles tus Forms actuales
        public int CompradorId
        {
            get => SolicitanteId;
            set => SolicitanteId = value;
        }

        public string OfertaTrueque
        {
            get => Trueque?.DescripcionOferta;
            set
            {
                if (Trueque == null) Trueque = new AcuerdoTrueque();
                Trueque.DescripcionOferta = value;
            }
        }

        // Tus propiedades calculadas originales
        public bool EsTrueque => Tipo == TipoSolicitud.Trueque;
        public bool EstaPendiente => Estado == EstadoSolicitud.Pendiente;
        public bool TruequeConfirmado => EsTrueque && (Trueque?.EstaCompletado ?? false);
    }
}