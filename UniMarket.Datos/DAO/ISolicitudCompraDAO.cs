using System.Collections.Generic;
using UniMarket.Entidades;

namespace UniMarket.Datos.DAO
{
    public interface ISolicitudCompraDAO
    {
        int Insertar(SolicitudCompra solicitud);
        SolicitudCompra ObtenerPorId(int id);
        bool ExistePendiente(int publicacionId, int compradorId);
        bool Aceptar(int id);
        bool Rechazar(int id);
        List<SolicitudDetalle> ObtenerRecibidas(int vendedorId);
        List<SolicitudDetalle> ObtenerEnviadas(int compradorId);
    }
}