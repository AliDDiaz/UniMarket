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
                    con.Open();
                    using (var tx = con.BeginTransaction())
                    {
                        try
                        {
                            string sqlSolicitud = @"INSERT INTO public.solicitudes (publicacion_id, solicitante_id, cantidad, precio_unitario, tipo)
                                                    VALUES (@PublicacionId, @SolicitanteId, @Cantidad, @PrecioUnitario, @Tipo)
                                                    RETURNING id";
                            int solicitudId;
                            using (var cmd = new NpgsqlCommand(sqlSolicitud, con, tx))
                            {
                                cmd.Parameters.AddWithValue("@PublicacionId", s.PublicacionId);
                                cmd.Parameters.AddWithValue("@SolicitanteId", s.SolicitanteId);
                                cmd.Parameters.AddWithValue("@Cantidad", s.Cantidad);
                                cmd.Parameters.AddWithValue("@PrecioUnitario",
                                    s.Tipo == TipoSolicitud.Compra ? (object)s.PrecioUnitario : DBNull.Value);
                                cmd.Parameters.AddWithValue("@Tipo", s.Tipo.ToString());

                                solicitudId = Convert.ToInt32(cmd.ExecuteScalar());
                            }

                            if (s.EsTrueque)
                            {
                                string sqlTrueque = @"INSERT INTO public.trueques (solicitud_id, producto_ofrecido_id, descripcion_oferta)
                                                      VALUES (@SolicitudId, @ProductoOfrecidoId, @DescripcionOferta)";
                                using (var cmdTrueque = new NpgsqlCommand(sqlTrueque, con, tx))
                                {
                                    cmdTrueque.Parameters.AddWithValue("@SolicitudId", solicitudId);
                                    cmdTrueque.Parameters.AddWithValue("@ProductoOfrecidoId",
                                        (object)s.Trueque?.ProductoOfrecidoId ?? DBNull.Value);
                                    cmdTrueque.Parameters.AddWithValue("@DescripcionOferta",
                                        (object)s.OfertaTrueque ?? DBNull.Value);

                                    cmdTrueque.ExecuteNonQuery();
                                }
                            }

                            tx.Commit();
                            s.Id = solicitudId;
                            return solicitudId;
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
                throw new Exception($"Error BD al insertar solicitud: {ex.Message}", ex);
            }
        }

        public SolicitudCompra ObtenerPorId(int id)
        {
            try
            {
                using (var con = _conexion.ObtenerConexion())
                {
                    string sql = @"SELECT s.id, s.publicacion_id, s.solicitante_id, s.cantidad, s.precio_unitario, 
                                          s.tipo, s.estado, s.fecha,
                                          t.id AS trueque_id, t.producto_ofrecido_id, t.descripcion_oferta AS oferta_trueque,
                                          t.confirmado_solicitante, t.confirmado_dueno, t.estado_entrega, t.fecha_confirmacion
                                   FROM public.solicitudes s
                                   LEFT JOIN public.trueques t ON t.solicitud_id = s.id
                                   WHERE s.id = @Id";
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
                                       SELECT 1 FROM public.solicitudes
                                       WHERE publicacion_id = @PublicacionId
                                         AND solicitante_id = @SolicitanteId
                                         AND estado = 'Pendiente')";
                    using (var cmd = new NpgsqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@PublicacionId", publicacionId);
                        cmd.Parameters.AddWithValue("@SolicitanteId", compradorId);
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
                            string sqlEstado = @"UPDATE public.solicitudes
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

                            string sqlStock = @"UPDATE public.publicaciones p
                                                SET stock = p.stock - s.cantidad
                                                FROM public.solicitudes s
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
                    string sql = @"UPDATE public.solicitudes
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
            string sql = @"SELECT s.id, s.publicacion_id, pr.titulo, s.cantidad, s.precio_unitario, s.tipo, 
                                  t.descripcion_oferta AS oferta_trueque,
                                  t.confirmado_solicitante, t.confirmado_dueno, t.estado_entrega,
                                  s.estado, s.fecha,
                                  c.nombre AS contraparte,
                                  NULL::varchar AS telefono
                           FROM public.solicitudes s
                           INNER JOIN public.publicaciones p ON p.id = s.publicacion_id
                           INNER JOIN public.productos pr ON pr.id = p.producto_id
                           INNER JOIN public.usuarios c ON c.id = s.solicitante_id
                           LEFT JOIN public.trueques t ON t.solicitud_id = s.id
                           WHERE pr.propietario_id = @UsuarioId
                           ORDER BY s.id DESC";

            return ListarDetalle(sql, vendedorId);
        }

        public List<SolicitudDetalle> ObtenerEnviadas(int compradorId)
        {
            string sql = @"SELECT s.id, s.publicacion_id, pr.titulo, s.cantidad, s.precio_unitario, s.tipo, 
                                  t.descripcion_oferta AS oferta_trueque,
                                  t.confirmado_solicitante, t.confirmado_dueno, t.estado_entrega,
                                  s.estado, s.fecha,
                                  CASE WHEN s.estado = 'Aceptada' THEN v.nombre END AS contraparte,
                                  CASE WHEN s.estado = 'Aceptada' THEN v.telefono END AS telefono
                           FROM public.solicitudes s
                           INNER JOIN public.publicaciones p ON p.id = s.publicacion_id
                           INNER JOIN public.productos pr ON pr.id = p.producto_id
                           INNER JOIN public.usuarios v ON v.id = pr.propietario_id
                           LEFT JOIN public.trueques t ON t.solicitud_id = s.id
                           WHERE s.solicitante_id = @UsuarioId
                           ORDER BY s.id DESC";

            return ListarDetalle(sql, compradorId);
        }

        public bool ConfirmarTrueque(int solicitudId, int usuarioId)
        {
            try
            {
                using (var conn = _conexion.ObtenerConexion())
                {
                    if (conn.State != System.Data.ConnectionState.Open)
                        conn.Open();

                    string sql = @"
                        UPDATE public.trueques t
                        SET 
                            confirmado_solicitante = CASE WHEN s.solicitante_id = @usuarioId THEN true ELSE t.confirmado_solicitante END,
                            confirmado_dueno = CASE WHEN pr.propietario_id = @usuarioId THEN true ELSE t.confirmado_dueno END,
                            estado_entrega = CASE 
                                WHEN (CASE WHEN s.solicitante_id = @usuarioId THEN true ELSE t.confirmado_solicitante END) = true 
                                 AND (CASE WHEN pr.propietario_id = @usuarioId THEN true ELSE t.confirmado_dueno END) = true 
                                THEN 'Completado' 
                                ELSE 'En proceso' 
                            END,
                            fecha_confirmacion = CASE 
                                WHEN (CASE WHEN s.solicitante_id = @usuarioId THEN true ELSE t.confirmado_solicitante END) = true 
                                 AND (CASE WHEN pr.propietario_id = @usuarioId THEN true ELSE t.confirmado_dueno END) = true 
                                THEN now() 
                                ELSE t.fecha_confirmacion 
                            END
                        FROM public.solicitudes s
                        INNER JOIN public.publicaciones pub ON s.publicacion_id = pub.id
                        INNER JOIN public.productos pr ON pub.producto_id = pr.id
                        WHERE t.solicitud_id = s.id
                          AND s.id = @solicitudId
                          AND (s.solicitante_id = @usuarioId OR pr.propietario_id = @usuarioId);";

                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@solicitudId", solicitudId);
                        cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Error BD al confirmar trueque: {ex.Message}", ex);
            }
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

        private bool BoolONulo(NpgsqlDataReader r, string columna)
        {
            int posicion = r.GetOrdinal(columna);
            return !r.IsDBNull(posicion) && r.GetBoolean(posicion);
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
            ConfirmadoSolicitante = BoolONulo(r, "confirmado_solicitante"),
            ConfirmadoDueno = BoolONulo(r, "confirmado_dueno"),
            EstadoEntrega = TextoONulo(r, "estado_entrega"),
            Estado = (EstadoSolicitud)Enum.Parse(typeof(EstadoSolicitud), r["estado"].ToString()),
            Fecha = Convert.ToDateTime(r["fecha"]).ToLocalTime(),
            NombreContraparte = TextoONulo(r, "contraparte"),
            TelefonoContacto = TextoONulo(r, "telefono")
        };

        private SolicitudCompra Mapear(NpgsqlDataReader r)
        {
            var solicitud = new SolicitudCompra
            {
                Id = Convert.ToInt32(r["id"]),
                PublicacionId = Convert.ToInt32(r["publicacion_id"]),
                SolicitanteId = Convert.ToInt32(r["solicitante_id"]),
                Cantidad = Convert.ToInt32(r["cantidad"]),
                PrecioUnitario = PrecioONulo(r),
                Tipo = (TipoSolicitud)Enum.Parse(typeof(TipoSolicitud), r["tipo"].ToString()),
                Estado = (EstadoSolicitud)Enum.Parse(typeof(EstadoSolicitud), r["estado"].ToString()),
                Fecha = Convert.ToDateTime(r["fecha"]).ToLocalTime()
            };

            if (!r.IsDBNull(r.GetOrdinal("trueque_id")))
            {
                solicitud.Trueque = new AcuerdoTrueque
                {
                    Id = Convert.ToInt32(r["trueque_id"]),
                    SolicitudId = solicitud.Id,
                    ProductoOfrecidoId = r.IsDBNull(r.GetOrdinal("producto_ofrecido_id"))
                        ? (int?)null
                        : Convert.ToInt32(r["producto_ofrecido_id"]),
                    DescripcionOferta = TextoONulo(r, "oferta_trueque"),
                    ConfirmadoSolicitante = BoolONulo(r, "confirmado_solicitante"),
                    ConfirmadoDueno = BoolONulo(r, "confirmado_dueno"),
                    EstadoEntrega = TextoONulo(r, "estado_entrega"),
                    FechaConfirmacion = r.IsDBNull(r.GetOrdinal("fecha_confirmacion"))
                        ? (DateTime?)null
                        : Convert.ToDateTime(r["fecha_confirmacion"]).ToLocalTime()
                };
            }

            return solicitud;
        }
    }
}