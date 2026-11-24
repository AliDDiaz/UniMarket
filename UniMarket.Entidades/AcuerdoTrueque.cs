using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace UniMarket.Entidades
{
    public class AcuerdoTrueque
    {
        public int Id { get; set; }
        public int SolicitudId { get; set; }
        public int? ProductoOfrecidoId { get; set; }
        public string DescripcionOferta { get; set; }
        public bool ConfirmadoSolicitante { get; set; }
        public bool ConfirmadoDueno { get; set; }
        public string EstadoEntrega { get; set; } = "En proceso"; // 'En proceso', 'Completado', 'Cancelado'
        public DateTime? FechaConfirmacion { get; set; }

        public bool EstaCompletado => ConfirmadoSolicitante && ConfirmadoDueno;
    }
}