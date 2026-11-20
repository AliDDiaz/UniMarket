using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using UniMarket.Entidades;

namespace UniMarket.Datos
{
    public class BotDatos
    {
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["UniMarketDB"].ConnectionString;

        // 1. Guardar un nuevo mensaje del bot
        public void InsertarMensaje(MensajeBot msg)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                string sql = @"INSERT INTO mensajes_bot (usuario_id, titulo, mensaje, tipo, solicitud_id, leido)
                               VALUES (@usuarioId, @titulo, @mensaje, @tipo, @solicitudId, FALSE);";

                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@usuarioId", msg.UsuarioId);
                    cmd.Parameters.AddWithValue("@titulo", msg.Titulo);
                    cmd.Parameters.AddWithValue("@mensaje", msg.Mensaje);
                    cmd.Parameters.AddWithValue("@tipo", msg.Tipo);
                    cmd.Parameters.AddWithValue("@solicitudId", (object)msg.SolicitudId ?? DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // 2. Obtener todos los mensajes de un usuario (para FrmBot)
        public List<MensajeBot> ObtenerMensajesPorUsuario(int usuarioId)
        {
            var lista = new List<MensajeBot>();

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                string sql = @"SELECT id, usuario_id, titulo, mensaje, tipo, solicitud_id, leido, fecha
                               FROM mensajes_bot
                               WHERE usuario_id = @usuarioId
                               ORDER BY fecha DESC;";

                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new MensajeBot
                            {
                                Id = reader.GetInt32(0),
                                UsuarioId = reader.GetInt32(1),
                                Titulo = reader.GetString(2),
                                Mensaje = reader.GetString(3),
                                Tipo = reader.GetString(4),
                                SolicitudId = reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5),
                                Leido = reader.GetBoolean(6),
                                Fecha = reader.GetDateTime(7)
                            });
                        }
                    }
                }
            }
            return lista;
        }

        // 3. Traer solo los mensajes NO leídos (para el Timer de FrmPrincipal2)
        public List<MensajeBot> ObtenerNoLeidos(int usuarioId)
        {
            var lista = new List<MensajeBot>();

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                string sql = @"SELECT id, usuario_id, titulo, mensaje, tipo, solicitud_id, leido, fecha
                               FROM mensajes_bot
                               WHERE usuario_id = @usuarioId AND leido = FALSE
                               ORDER BY fecha ASC;";

                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new MensajeBot
                            {
                                Id = reader.GetInt32(0),
                                UsuarioId = reader.GetInt32(1),
                                Titulo = reader.GetString(2),
                                Mensaje = reader.GetString(3),
                                Tipo = reader.GetString(4),
                                SolicitudId = reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5),
                                Leido = reader.GetBoolean(6),
                                Fecha = reader.GetDateTime(7)
                            });
                        }
                    }
                }
            }
            return lista;
        }

        // 4. Marcar todos los mensajes del usuario como leídos al abrir FrmBot
        public void MarcarTodosComoLeidos(int usuarioId)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "UPDATE mensajes_bot SET leido = TRUE WHERE usuario_id = @usuarioId AND leido = FALSE;";

                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
