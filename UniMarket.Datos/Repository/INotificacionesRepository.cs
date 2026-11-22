using System.Collections.Generic;
using UniMarket.Entidades;

namespace UniMarket.Datos.Repository
{
    public interface INotificacionesRepository
    {
        void Add(MensajeNotificaciones mensaje);
        List<MensajeNotificaciones> GetByUser(int usuarioId);
        List<MensajeNotificaciones> GetUnread(int usuarioId);
        void MarkAllAsRead(int usuarioId);
    }
}