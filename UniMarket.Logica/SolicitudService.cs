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
        private readonly IUsuarioRepository _usuarios;
        private readonly NotificacionesService _notificaciones;

        public SolicitudService(ISolicitudCompraRepository solicitudes,
                                IPublicacionRepository publicaciones,
                                IUsuarioRepository usuarios)
        {
            _solicitudes = solicitudes;
            _publicaciones = publicaciones;
            _usuarios = usuarios;
            _notificaciones = new NotificacionesService();
        }

        // Constructor de conveniencia
        public SolicitudService()
            : this(new SolicitudCompraRepository(new SolicitudCompraDAO()),
                   new PublicacionRepository(new PublicacionDAO()),
                   new UsuarioRepository(new UsuarioDAO()))
        { }

        public int Solicitar(int publicacionId, int compradorId, int cantidad)
        {
            if (compradorId <= 0)
                throw new Exception("Debes iniciar sesión para solicitar una compra.");

            if (cantidad <= 0)
                throw new Exception("La cantidad debe ser mayor a cero.");

            var publicacion = _publicaciones.GetById(publicacionId);

            if (publicacion == null || !publicacion.Activo)
                throw new Exception("La publicación no existe.");

            if (!publicacion.AceptaVenta)
                throw new Exception("Esta publicación solo acepta trueque.");

            if (publicacion.PerteneceA(compradorId))
                throw new Exception("No puedes comprar tu propia publicación.");

            if (publicacion.Stock <= 0)
                throw new Exception("Esta publicación está agotada.");

            if (!publicacion.TieneStockPara(cantidad))
                throw new Exception($"Solo hay {publicacion.Stock} unidad(es) disponible(s).");

            if (_solicitudes.HasPending(publicacionId, compradorId))
                throw new Exception("Ya tienes una solicitud pendiente para esta publicación.");

            var comprador = _usuarios.GetById(compradorId);
            if(comprador == null)
            {
                throw new Exception("No se encontró tu usuario. Inicia sesión de nuevo.");
            }
            var solicitud = new SolicitudCompra
            {
                PublicacionId = publicacionId,
                CompradorId = compradorId,
                Cantidad = cantidad,
                PrecioUnitario = publicacion.Precio
            };

            int nuevaSolicitudId = _solicitudes.Add(solicitud);

            decimal totalOferta = publicacion.Precio * cantidad;

            _notificaciones.NotificarNuevaOferta(
                vendedorId: publicacion.VendedorId,
                nombreComprador: comprador.Nombre,
                tituloProducto: publicacion.Titulo,
                cantidad: cantidad,
                totalOferta: totalOferta,
                solicitudId: nuevaSolicitudId
            );

            return nuevaSolicitudId;
        }

        // Envía cada producto del carrito por separado: un fallo no detiene a los demás
        public ResultadoEnvioCarrito EnviarCarrito(int compradorId, IEnumerable<ItemCarrito> items)
        {
            var resultado = new ResultadoEnvioCarrito();

            foreach (var item in items)
            {
                try
                {
                    Solicitar(item.Producto.Id, compradorId, item.Cantidad);
                    resultado.Enviados.Add(item.Producto.Id);
                }
                catch (Exception ex)
                {
                    resultado.Fallidos.Add($"{item.Producto.Titulo}: {ex.Message}");
                }
            }

            return resultado;
        }

        public void Aceptar(int solicitudId, int usuarioId)
        {
            var solicitud = ObtenerPendienteDelVendedor(solicitudId, usuarioId, out Publicacion publicacion);

            if (!publicacion.Activo)
                throw new Exception("La publicación ya no está disponible.");

            if (!_solicitudes.Accept(solicitudId))
                throw new Exception(
                    "No se pudo aceptar: la solicitud ya fue atendida o ya no hay unidades suficientes.");

            _notificaciones.NotificarRespuestaOferta(
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

            _notificaciones.NotificarRespuestaOferta(
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

    public class ResultadoEnvioCarrito
    {
        public List<int> Enviados { get; } = new List<int>();     // ids de las publicaciones enviadas
        public List<string> Fallidos { get; } = new List<string>(); // "Producto: motivo"
    }

}