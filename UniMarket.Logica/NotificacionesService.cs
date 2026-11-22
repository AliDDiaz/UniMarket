using System;
using System.Collections.Generic;
using System.Diagnostics;
using UniMarket.Datos.DAO;
using UniMarket.Datos.Repository;
using UniMarket.Entidades;

namespace UniMarket.Logica
{
    public class NotificacionesService
    {
        private readonly INotificacionesRepository _notificaciones;

        public NotificacionesService(INotificacionesRepository notificaciones)
        {
            _notificaciones = notificaciones;
        }
            
        // Constructor de conveniencia
        public NotificacionesService()
            : this(new NotificacionesRepository(new NotificacionesDAO()))
        { }

        // Avisa al vendedor cuando recibe una oferta de compra
        public void NotificarNuevaOferta(int vendedorId, string nombreComprador,
            string tituloProducto, int cantidad, decimal totalOferta, int? solicitudId = null)
        {
            Guardar(new MensajeNotificaciones
            {
                UsuarioId = vendedorId,
                Titulo = "Nueva solicitud de compra",
                Mensaje = $"¡Hola! {nombreComprador} quiere comprar {cantidad} unidad(es) de '{tituloProducto}' por un total de ${totalOferta:N0}. Revisa tus solicitudes para aceptar o rechazar.",
                Tipo = "OFERTA_RECIBIDA",
                SolicitudId = solicitudId
            });
        }

        // Avisa al comprador si el vendedor aceptó o rechazó su solicitud
        public void NotificarRespuestaOferta(int compradorId, string tituloProducto,
            bool aceptada, int? solicitudId = null)
        {
            Guardar(new MensajeNotificaciones
            {
                UsuarioId = compradorId,
                Titulo = aceptada ? "¡Solicitud aceptada!" : "Solicitud rechazada",
                Mensaje = aceptada
                    ? $"¡Buenas noticias! El vendedor aceptó tu solicitud por '{tituloProducto}'. Ya puedes coordinar la entrega."
                    : $"El vendedor rechazó tu solicitud por '{tituloProducto}'. Puedes seguir explorando otras publicaciones.",
                Tipo = aceptada ? "OFERTA_ACEPTADA" : "OFERTA_RECHAZADA",
                SolicitudId = solicitudId
            });
        }

        public List<MensajeNotificaciones> ObtenerHistorial(int usuarioId)
            => _notificaciones.GetByUser(usuarioId);

        public List<MensajeNotificaciones> RevisarNuevosMensajes(int usuarioId)
            => _notificaciones.GetUnread(usuarioId);

        public void MarcarComoLeidos(int usuarioId)
            => _notificaciones.MarkAllAsRead(usuarioId);

        // Una notificación fallida NO debe tumbar la operación principal
        // (la solicitud ya quedó guardada), pero sí deja rastro del error.
        private void Guardar(MensajeNotificaciones mensaje)
        {
            try
            {
                _notificaciones.Add(mensaje);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Notificaciones] No se pudo guardar '{mensaje.Tipo}' " +
                                $"para el usuario {mensaje.UsuarioId}: {ex.Message}");
            }
        }
    }
}