using System;
using System.Collections.Generic;
using UniMarket.Datos.DAO;
using UniMarket.Datos.Repository;
using UniMarket.Entidades;

namespace UniMarket.Logica
{
    public class SolicitudService
    {
        private readonly ISolicitudCompraRepository _solicitudes;
        private readonly IPublicacionRepository _publicaciones;
        private readonly BotService _bot;

        public SolicitudService(ISolicitudCompraRepository solicitudes,
                                IPublicacionRepository publicaciones)
        {
            _solicitudes = solicitudes;
            _publicaciones = publicaciones;
            _bot = new BotService();
        }

        // Constructor de conveniencia
        public SolicitudService()
            : this(new SolicitudCompraRepository(new SolicitudCompraDAO()),
                   new PublicacionRepository(new PublicacionDAO()))
        { }

        public int Solicitar(int publicacionId, int compradorId, int cantidad, string nombreComprador = "Un usuario")
        {
            if (compradorId <= 0)
                throw new Exception("Debes iniciar sesión para solicitar una compra.");

            if (cantidad <= 0)
                throw new Exception("La cantidad debe ser mayor a cero.");

            var publicacion = _publicaciones.GetById(publicacionId);

            if (publicacion == null || !publicacion.Activo)
                throw new Exception("La publicación no existe.");

            if (publicacion.PerteneceA(compradorId))
                throw new Exception("No puedes comprar tu propia publicación.");

            if (publicacion.Stock <= 0)
                throw new Exception("Esta publicación está agotada.");

            if (!publicacion.TieneStockPara(cantidad))
                throw new Exception($"Solo hay {publicacion.Stock} unidad(es) disponible(s).");

            if (_solicitudes.HasPending(publicacionId, compradorId))
                throw new Exception("Ya tienes una solicitud pendiente para esta publicación.");

            var solicitud = new SolicitudCompra
            {
                PublicacionId = publicacionId,
                CompradorId = compradorId,
                Cantidad = cantidad
            };

            int nuevaSolicitudId = _solicitudes.Add(solicitud);

            decimal totalOferta = publicacion.Precio * cantidad;

            _bot.NotificarNuevaOferta(
                vendedorId: publicacion.VendedorId,
                nombreComprador: nombreComprador,
                tituloProducto: publicacion.Titulo,
                cantidad: cantidad,
                totalOferta: totalOferta,
                solicitudId: nuevaSolicitudId
            );

            return nuevaSolicitudId;
        }

        public void Aceptar(int solicitudId, int usuarioId)
        {
            var solicitud = ObtenerPendienteDelVendedor(solicitudId, usuarioId, out Publicacion publicacion);

            if (!publicacion.Activo)
                throw new Exception("La publicación ya no está disponible.");

            if (!_solicitudes.Accept(solicitudId))
                throw new Exception(
                    "No se pudo aceptar: la solicitud ya fue atendida o ya no hay unidades suficientes.");

            _bot.NotificarRespuestaOferta(
                compradorId: solicitud.CompradorId,
                tituloProducto: publicacion.Titulo,
                aceptada: true,
                solicitudId: solicitudId
            );
        }

        public void Rechazar(int solicitudId, int usuarioId)
        {
            var solicitud = ObtenerPendienteDelVendedor(solicitudId, usuarioId, out Publicacion publicacion);

            if (!_solicitudes.Reject(solicitudId))
                throw new Exception("No se pudo rechazar: la solicitud ya fue atendida.");

            _bot.NotificarRespuestaOferta(
                compradorId: solicitud.CompradorId,
                tituloProducto: publicacion.Titulo,
                aceptada: false,
                solicitudId: solicitudId
            );
        }

        public List<SolicitudDetalle> ObtenerRecibidas(int vendedorId)
        {
            if (vendedorId <= 0)
                throw new Exception("Debes iniciar sesión para ver tus solicitudes.");

            return _solicitudes.GetReceived(vendedorId);
        }

        public List<SolicitudDetalle> ObtenerEnviadas(int compradorId)
        {
            if (compradorId <= 0)
                throw new Exception("Debes iniciar sesión para ver tus solicitudes.");

            return _solicitudes.GetSent(compradorId);
        }

        private SolicitudCompra ObtenerPendienteDelVendedor(
            int solicitudId, int usuarioId, out Publicacion publicacion)
        {
            publicacion = null;

            if (solicitudId <= 0 || usuarioId <= 0)
                throw new Exception("La solicitud no es válida.");

            var solicitud = _solicitudes.GetById(solicitudId);

            if (solicitud == null)
                throw new Exception("La solicitud no existe.");

            publicacion = _publicaciones.GetById(solicitud.PublicacionId);

            if (publicacion == null || !publicacion.PerteneceA(usuarioId))
                throw new Exception("Solo el vendedor puede atender esta solicitud.");

            if (!solicitud.EstaPendiente)
                throw new Exception("Esta solicitud ya fue atendida.");

            return solicitud;
        }
    }
}