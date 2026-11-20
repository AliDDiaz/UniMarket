using System;
using System.Collections.Generic;
using UniMarket.Datos;
using UniMarket.Entidades;

namespace UniMarket.Logica
{
    public class NotificacionesService
    {
        private readonly NotificacionesDatos _notificaionesDatos = new NotificacionesDatos();

        // Avisa al vendedor cuando recibe una oferta de compra
        // En BotService.cs (Capa Logica)
        public void NotificarNuevaOferta(int vendedorId, string nombreComprador, string tituloProducto, int cantidad, decimal totalOferta, int? solicitudId = null)
        {
            try
            {
                var aviso = new MensajeNotificaciones
                {
                    UsuarioId = vendedorId,
                    Titulo = "Nueva solicitud de compra",
                    Mensaje = $"¡Hola! {nombreComprador} quiere comprar {cantidad} unidad(es) de '{tituloProducto}' por un total de ${totalOferta:N0}. Revisa tus solicitudes para aceptar o rechazar.",
                    Tipo = "OFERTA_RECIBIDA",
                    SolicitudId = solicitudId
                };

                _notificaionesDatos.InsertarMensaje(aviso);
            }
            catch
            {
            }
        }

        public void NotificarRespuestaOferta(int compradorId, string tituloProducto, bool aceptada, int? solicitudId = null)
        {
            try
            {
                string titulo = aceptada ? "¡Solicitud aceptada!" : "Solicitud rechazada";
                string detalle = aceptada
                    ? $"¡Buenas noticias! El vendedor aceptó tu solicitud por '{tituloProducto}'. Ya puedes coordinar la entrega."
                    : $"El vendedor rechazó tu solicitud por '{tituloProducto}'. Puedes seguir explorando otras publicaciones.";

                var aviso = new MensajeNotificaciones
                {
                    UsuarioId = compradorId,
                    Titulo = titulo,
                    Mensaje = detalle,
                    Tipo = aceptada ? "OFERTA_ACEPTADA" : "OFERTA_RECHAZADA",
                    SolicitudId = solicitudId
                };

                _notificaionesDatos.InsertarMensaje(aviso);
            }
            catch
            {
            }
        }

        public List<MensajeNotificaciones> ObtenerHistorial(int usuarioId)
        {
            return _notificaionesDatos.ObtenerMensajesPorUsuario(usuarioId);
        }

        public List<MensajeNotificaciones> RevisarNuevosMensajes(int usuarioId)
        {
            return _notificaionesDatos.ObtenerNoLeidos(usuarioId);
        }

        public void MarcarComoLeidos(int usuarioId)
        {
            _notificaionesDatos.MarcarTodosComoLeidos(usuarioId);
        }
    }
}