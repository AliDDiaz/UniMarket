using System.Collections.Generic;
using UniMarket.Datos.DAO;
using UniMarket.Entidades;

namespace UniMarket.Datos.Repository
{
    public class SolicitudCompraRepository : ISolicitudCompraRepository
    {
        private readonly ISolicitudCompraDAO _dao;

        public SolicitudCompraRepository(ISolicitudCompraDAO dao)
        {
            _dao = dao;
        }

        public int Add(SolicitudCompra s) => _dao.Insertar(s);
        public SolicitudCompra GetById(int id) => _dao.ObtenerPorId(id);
        public bool HasPending(int publicacionId, int compradorId) => _dao.ExistePendiente(publicacionId, compradorId);
        public bool Accept(int id) => _dao.Aceptar(id);
        public bool Reject(int id) => _dao.Rechazar(id);
        public List<SolicitudDetalle> GetReceived(int vendedorId) => _dao.ObtenerRecibidas(vendedorId);
        public List<SolicitudDetalle> GetSent(int compradorId) => _dao.ObtenerEnviadas(compradorId);
    }
}