using Npgsql;
using System;
using System.Collections.Generic;
using UniMarket.Entidades;

namespace UniMarket.Datos.DAO
{
    public class NotificacionesDAO : INotificacionesDAO
    {
        private const string Columnas =
            "id, usuario_id, titulo, mensaje, tipo, solicitud_id, leido, fecha";

        private readonly Conexion _conexion;

        public NotificacionesDAO() { _conexion = new Conexion(); }

        public void Insertar(MensajeNotificaciones mensaje)
        {
            try
            {
                using (var con = _conexion.ObtenerConexion())
                {
                    string sql = @"INSERT INTO mensajes_notificaciones
                                       (usuario_id, titulo, mensaje, tipo, solicitud_id, leido)
                                   VALUES (@UsuarioId, @Titulo, @Mensaje, @Tipo, @SolicitudId, FALSE)";
                    using (var cmd = new NpgsqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@UsuarioId", mensaje.UsuarioId);
                        cmd.Parameters.AddWithValue("@Titulo", mensaje.Titulo);
                        cmd.Parameters.AddWithValue("@Mensaje", mensaje.Mensaje);
                        cmd.Parameters.AddWithValue("@Tipo", mensaje.Tipo);
                        cmd.Parameters.AddWithValue("@SolicitudId",
                            (object)mensaje.SolicitudId ?? DBNull.Value);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Error BD al guardar la notificación: {ex.Message}", ex);
            }
        }

        public List<MensajeNotificaciones> ObtenerPorUsuario(int usuarioId)
        {
            string sql = $@"SELECT {Columnas}
                            FROM mensajes_notificaciones
                            WHERE usuario_id = @UsuarioId
                            ORDER BY fecha DESC";
            return Listar(sql, usuarioId);
        }

        public List<MensajeNotificaciones> ObtenerNoLeidos(int usuarioId)
        {
            string sql = $@"SELECT {Columnas}
                            FROM mensajes_notificaciones
                            WHERE usuario_id = @UsuarioId AND leido = FALSE
                            ORDER BY fecha ASC";
            return Listar(sql, usuarioId);
        }

        public void MarcarTodosComoLeidos(int usuarioId)
        {
            try
            {
                using (var con = _conexion.ObtenerConexion())
                {
                    string sql = @"UPDATE mensajes_notificaciones
                                   SET leido = TRUE
                                   WHERE usuario_id = @UsuarioId AND leido = FALSE";
                    using (var cmd = new NpgsqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@UsuarioId", usuarioId);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Error BD al marcar notificaciones como leídas: {ex.Message}", ex);
            }
        }

        // Un solo lugar para ejecutar la consulta y mapear (antes estaba copiado dos veces)
        private List<MensajeNotificaciones> Listar(string sql, int usuarioId)
        {
            var lista = new List<MensajeNotificaciones>();
            try
            {
                using (var con = _conexion.ObtenerConexion())
                using (var cmd = new NpgsqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@UsuarioId", usuarioId);
                    con.Open();
                    using (var r = cmd.ExecuteReader())
                        while (r.Read()) lista.Add(Mapear(r));
                }
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Error BD al listar notificaciones: {ex.Message}", ex);
            }
            return lista;
        }

        private MensajeNotificaciones Mapear(NpgsqlDataReader r) => new MensajeNotificaciones
        {
            Id = Convert.ToInt32(r["id"]),
            UsuarioId = Convert.ToInt32(r["usuario_id"]),
            Titulo = r["titulo"].ToString(),
            Mensaje = r["mensaje"].ToString(),
            Tipo = r["tipo"].ToString(),
            SolicitudId = r["solicitud_id"] is DBNull ? (int?)null : Convert.ToInt32(r["solicitud_id"]),
            Leido = !(r["leido"] is DBNull) && Convert.ToBoolean(r["leido"]),
            Fecha = Convert.ToDateTime(r["fecha"]).ToLocalTime()
        };
    }
}