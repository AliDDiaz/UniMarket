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

        // Avisa al vendedor cuando recibe una propuesta de trueque
        public void NotificarNuevaPropuestaTrueque(int vendedorId, string nombreComprador,
            string tituloProducto, int cantidad, string oferta, int? solicitudId = null)
        {
            // La tarjeta de notificación tiene poco espacio: se acorta la oferta
            string resumen = oferta.Length > 100 ? oferta.Substring(0, 100) + "…" : oferta;

            Guardar(new MensajeNotificaciones
            {
                UsuarioId = vendedorId,
                Titulo = "Nueva propuesta de trueque",
                Mensaje = $"¡Hola! {nombreComprador} te propone un trueque por {cantidad} unidad(es) de '{tituloProducto}'. " +
                          $"Ofrece: \"{resumen}\". Revisa tus solicitudes para aceptar o rechazar.",
                Tipo = "TRUEQUE_RECIBIDO",
                SolicitudId = solicitudId
            });
        }

        // Avisa al comprador si el vendedor aceptó o rechazó su solicitud (compra o trueque)
        public void NotificarRespuestaOferta(int compradorId, string tituloProducto,
            bool aceptada, int? solicitudId = null, bool esTrueque = false)
        {
            string titulo;
            string detalle;

            if (esTrueque)
            {
                titulo = aceptada ? "¡Propuesta de trueque aceptada!" : "Propuesta de trueque rechazada";
                detalle = aceptada
                    ? $"¡Buenas noticias! El vendedor aceptó tu propuesta de trueque por '{tituloProducto}'. Ya puedes coordinar el intercambio."
                    : $"El vendedor rechazó tu propuesta de trueque por '{tituloProducto}'. Puedes proponer otro objeto o explorar otras publicaciones.";
            }
            else
            {
                titulo = aceptada ? "¡Solicitud aceptada!" : "Solicitud rechazada";
                detalle = aceptada
                    ? $"¡Buenas noticias! El vendedor aceptó tu solicitud por '{tituloProducto}'. Ya puedes coordinar la entrega."
                    : $"El vendedor rechazó tu solicitud por '{tituloProducto}'. Puedes seguir explorando otras publicaciones.";
            }

            Guardar(new MensajeNotificaciones
            {
                UsuarioId = compradorId,
                Titulo = titulo,
                Mensaje = detalle,
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