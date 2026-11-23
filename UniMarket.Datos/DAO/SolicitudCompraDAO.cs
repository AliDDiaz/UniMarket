using Npgsql;
using System;
using System.Collections.Generic;
using UniMarket.Entidades;

namespace UniMarket.Datos.DAO
{
    public class SolicitudCompraDAO : ISolicitudCompraDAO
    {
        private readonly Conexion _conexion;

        public SolicitudCompraDAO() { _conexion = new Conexion(); }

        public int Insertar(SolicitudCompra s)
        {
            try
            {
                using (var con = _conexion.ObtenerConexion())
                {
                    string sql = @"INSERT INTO solicitudes_compra (publicacion_id, comprador_id, cantidad, precio_unitario, tipo, oferta_trueque)
                                   VALUES (@PublicacionId, @CompradorId, @Cantidad, @PrecioUnitario, @Tipo, @OfertaTrueque)
                                   RETURNING id";
                    using (var cmd = new NpgsqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@PublicacionId", s.PublicacionId);
                        cmd.Parameters.AddWithValue("@CompradorId", s.CompradorId);
                        cmd.Parameters.AddWithValue("@Cantidad", s.Cantidad);
                        cmd.Parameters.AddWithValue("@PrecioUnitario",
                            s.Tipo == TipoSolicitud.Compra ? (object)s.PrecioUnitario : DBNull.Value);
                        cmd.Parameters.AddWithValue("@Tipo", s.Tipo.ToString());
                        cmd.Parameters.AddWithValue("@OfertaTrueque", (object)s.OfertaTrueque ?? DBNull.Value);
                        con.Open();
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Error BD al insertar solicitud: {ex.Message}", ex);
            }
        }

        public SolicitudCompra ObtenerPorId(int id)
        {
            try
            {
                using (var con = _conexion.ObtenerConexion())
                {
                    string sql = @"SELECT id, publicacion_id, comprador_id, cantidad, precio_unitario, tipo, oferta_trueque, estado, fecha
                                   FROM solicitudes_compra WHERE id = @Id";
                    using (var cmd = new NpgsqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        con.Open();
                        using (var r = cmd.ExecuteReader())
                            if (r.Read()) return Mapear(r);
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Error BD al buscar solicitud: {ex.Message}", ex);
            }
            return null;
        }

        public bool ExistePendiente(int publicacionId, int compradorId)
        {
            try
            {
                using (var con = _conexion.ObtenerConexion())
                {
                    string sql = @"SELECT EXISTS (
                                       SELECT 1 FROM solicitudes_compra
                                       WHERE publicacion_id = @PublicacionId
                                         AND comprador_id = @CompradorId
                                         AND estado = 'Pendiente')";
                    using (var cmd = new NpgsqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@PublicacionId", publicacionId);
                        cmd.Parameters.AddWithValue("@CompradorId", compradorId);
                        con.Open();
                        return Convert.ToBoolean(cmd.ExecuteScalar());
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Error BD al comprobar solicitudes: {ex.Message}", ex);
            }
        }

        public bool Aceptar(int id)
        {
            try
            {
                using (var con = _conexion.ObtenerConexion())
                {
                    con.Open();
                    using (var tx = con.BeginTransaction())
                    {
                        try
                        {
                            // 1. Cambiar el estado, solo si todavía está pendiente
                            string sqlEstado = @"UPDATE solicitudes_compra
                                                 SET estado = 'Aceptada'
                                                 WHERE id = @Id AND estado = 'Pendiente'";
                            using (var cmd = new NpgsqlCommand(sqlEstado, con, tx))
                            {
                                cmd.Parameters.AddWithValue("@Id", id);
                                if (cmd.ExecuteNonQuery() != 1)
                                {
                                    tx.Rollback();
                                    return false;
                                }
                            }

                            // 2. Descontar el stock, solo si alcanza
                            string sqlStock = @"UPDATE publicaciones p
                                                SET stock = p.stock - s.cantidad
                                                FROM solicitudes_compra s
                                                WHERE s.id = @Id
                                                  AND p.id = s.publicacion_id
                                                  AND p.stock >= s.cantidad";
                            using (var cmd = new NpgsqlCommand(sqlStock, con, tx))
                            {
                                cmd.Parameters.AddWithValue("@Id", id);
                                if (cmd.ExecuteNonQuery() != 1)
                                {
                                    tx.Rollback();
                                    return false;
                                }
                            }

                            tx.Commit();
                            return true;
                        }
                        catch
                        {
                            tx.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Error BD al aceptar solicitud: {ex.Message}", ex);
            }
        }

        public bool Rechazar(int id)
        {
            try
            {
                using (var con = _conexion.ObtenerConexion())
                {
                    string sql = @"UPDATE solicitudes_compra
                                   SET estado = 'Rechazada'
                                   WHERE id = @Id AND estado = 'Pendiente'";
                    using (var cmd = new NpgsqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        con.Open();
                        return cmd.ExecuteNonQuery() == 1;
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Error BD al rechazar solicitud: {ex.Message}", ex);
            }
        }

        public List<SolicitudDetalle> ObtenerRecibidas(int vendedorId)
        {
            string sql = @"SELECT s.id, s.publicacion_id, p.titulo, s.cantidad, s.precio_unitario, s.tipo, s.oferta_trueque,
                          s.estado, s.fecha,
                          c.nombre AS contraparte,
                          NULL::varchar AS telefono
                   FROM solicitudes_compra s
                   JOIN publicaciones p ON p.id = s.publicacion_id
                   JOIN usuarios c ON c.id = s.comprador_id
                   WHERE p.vendedor_id = @UsuarioId
                   ORDER BY s.id DESC";

            return ListarDetalle(sql, vendedorId);
        }

        public List<SolicitudDetalle> ObtenerEnviadas(int compradorId)
        {
            string sql = @"SELECT s.id, s.publicacion_id, p.titulo, s.cantidad, s.precio_unitario, s.tipo, s.oferta_trueque,
                          s.estado, s.fecha,
                          CASE WHEN s.estado = 'Aceptada' THEN v.nombre END AS contraparte,
                          CASE WHEN s.estado = 'Aceptada' THEN v.telefono END AS telefono
                   FROM solicitudes_compra s
                   JOIN publicaciones p ON p.id = s.publicacion_id
                   JOIN usuarios v ON v.id = p.vendedor_id
                   WHERE s.comprador_id = @UsuarioId
                   ORDER BY s.id DESC";

            return ListarDetalle(sql, compradorId);
        }

        private List<SolicitudDetalle> ListarDetalle(string sql, int usuarioId)
        {
            var lista = new List<SolicitudDetalle>();
            try
            {
                using (var con = _conexion.ObtenerConexion())
                using (var cmd = new NpgsqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@UsuarioId", usuarioId);
                    con.Open();
                    using (var r = cmd.ExecuteReader())
                        while (r.Read()) lista.Add(MapearDetalle(r));
                }
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Error BD al listar solicitudes: {ex.Message}", ex);
            }
            return lista;
        }

        private string TextoONulo(NpgsqlDataReader r, string columna)
        {
            int posicion = r.GetOrdinal(columna);
            return r.IsDBNull(posicion) ? null : r.GetString(posicion);
        }

        private decimal PrecioONulo(NpgsqlDataReader r)
        {
            int posicion = r.GetOrdinal("precio_unitario");
            return r.IsDBNull(posicion) ? 0m : r.GetDecimal(posicion);
        }

        private SolicitudDetalle MapearDetalle(NpgsqlDataReader r) => new SolicitudDetalle
        {
            Id = Convert.ToInt32(r["id"]),
            PublicacionId = Convert.ToInt32(r["publicacion_id"]),
            Titulo = r["titulo"].ToString(),
            Cantidad = Convert.ToInt32(r["cantidad"]),
            PrecioUnitario = PrecioONulo(r),
            Tipo = (TipoSolicitud)Enum.Parse(typeof(TipoSolicitud), r["tipo"].ToString()),
            OfertaTrueque = TextoONulo(r, "oferta_trueque"),
            Estado = (EstadoSolicitud)Enum.Parse(typeof(EstadoSolicitud), r["estado"].ToString()),
            Fecha = Convert.ToDateTime(r["fecha"]).ToLocalTime(),
            NombreContraparte = TextoONulo(r, "contraparte"),
            TelefonoContacto = TextoONulo(r, "telefono")
        };

        private SolicitudCompra Mapear(NpgsqlDataReader r) => new SolicitudCompra
        {
            Id = Convert.ToInt32(r["id"]),
            PublicacionId = Convert.ToInt32(r["publicacion_id"]),
            CompradorId = Convert.ToInt32(r["comprador_id"]),
            Cantidad = Convert.ToInt32(r["cantidad"]),
            PrecioUnitario = PrecioONulo(r),
            Tipo = (TipoSolicitud)Enum.Parse(typeof(TipoSolicitud), r["tipo"].ToString()),
            OfertaTrueque = TextoONulo(r, "oferta_trueque"),
            Estado = (EstadoSolicitud)Enum.Parse(typeof(EstadoSolicitud), r["estado"].ToString()),
            Fecha = Convert.ToDateTime(r["fecha"]).ToLocalTime()
        };
    }
}