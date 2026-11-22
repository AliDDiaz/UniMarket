using System.Collections.Generic;
using UniMarket.Datos.DAO;
using UniMarket.Entidades;

namespace UniMarket.Datos.Repository
{
    public class NotificacionesRepository : INotificacionesRepository
    {
        private readonly INotificacionesDAO _dao;

        public NotificacionesRepository(INotificacionesDAO dao)
        {
            _dao = dao;
        }

        public void Add(MensajeNotificaciones mensaje) => _dao.Insertar(mensaje);
        public List<MensajeNotificaciones> GetByUser(int usuarioId) => _dao.ObtenerPorUsuario(usuarioId);
        public List<MensajeNotificaciones> GetUnread(int usuarioId) => _dao.ObtenerNoLeidos(usuarioId);
        public void MarkAllAsRead(int usuarioId) => _dao.MarcarTodosComoLeidos(usuarioId);
    }
}