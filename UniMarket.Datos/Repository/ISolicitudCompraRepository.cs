using System.Collections.Generic;
using UniMarket.Entidades;

namespace UniMarket.Datos.Repository
{
    public interface ISolicitudCompraRepository
    {
        int Add(SolicitudCompra solicitud);
        SolicitudCompra GetById(int id);
        bool HasPending(int publicacionId, int compradorId);
        bool Accept(int id);
        bool Reject(int id);
        List<SolicitudDetalle> GetReceived(int vendedorId);
        List<SolicitudDetalle> GetSent(int compradorId);
    }
}