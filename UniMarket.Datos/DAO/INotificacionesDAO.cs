using System.Collections.Generic;
using UniMarket.Entidades;

namespace UniMarket.Datos.DAO
{
    public interface INotificacionesDAO
    {
        void Insertar(MensajeNotificaciones mensaje);
        List<MensajeNotificaciones> ObtenerPorUsuario(int usuarioId);
        List<MensajeNotificaciones> ObtenerNoLeidos(int usuarioId);
        void MarcarTodosComoLeidos(int usuarioId);
    }
}