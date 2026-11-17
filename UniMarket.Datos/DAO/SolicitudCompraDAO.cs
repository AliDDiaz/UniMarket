using System;
using Npgsql;
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
                    string sql = @"INSERT INTO solicitudes_compra (publicacion_id, comprador_id, cantidad)
                                   VALUES (@PublicacionId, @CompradorId, @Cantidad)
                                   RETURNING id";
                    using (var cmd = new NpgsqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@PublicacionId", s.PublicacionId);
                        cmd.Parameters.AddWithValue("@CompradorId", s.CompradorId);
                        cmd.Parameters.AddWithValue("@Cantidad", s.Cantidad);
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
                    string sql = @"SELECT id, publicacion_id, comprador_id, cantidad, estado, fecha
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

        private SolicitudCompra Mapear(NpgsqlDataReader r) => new SolicitudCompra
        {
            Id = Convert.ToInt32(r["id"]),
            PublicacionId = Convert.ToInt32(r["publicacion_id"]),
            CompradorId = Convert.ToInt32(r["comprador_id"]),
            Cantidad = Convert.ToInt32(r["cantidad"]),
            Estado = r["estado"].ToString(),
            Fecha = Convert.ToDateTime(r["fecha"])
        };
    }
}