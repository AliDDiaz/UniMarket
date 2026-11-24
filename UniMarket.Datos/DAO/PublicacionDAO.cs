using System;
using System.Collections.Generic;
using Npgsql;
using NpgsqlTypes;
using UniMarket.Entidades;

namespace UniMarket.Datos.DAO
{
    public class PublicacionDAO : IPublicacionDAO
    {
        private const string SelectBase = @"
            SELECT pub.id, pub.producto_id, pr.propietario_id, pr.titulo, pr.descripcion, 
                   fp.imagen, pv.precio, pub.stock, pub.activo, pub.modalidad, pt.trueque_busca
            FROM public.publicaciones pub
            INNER JOIN public.productos pr ON pub.producto_id = pr.id
            LEFT JOIN public.publicaciones_venta pv ON pv.id_publicacion = pub.id
            LEFT JOIN public.publicaciones_trueque pt ON pt.id_publicacion = pub.id
            LEFT JOIN LATERAL (
                SELECT imagen 
                FROM public.fotos_productos 
                WHERE id_producto = pr.id 
                ORDER BY id ASC 
                LIMIT 1
            ) fp ON TRUE";

        private readonly Conexion _conexion;

        public PublicacionDAO()
        {
            _conexion = new Conexion();
        }

        public List<Publicacion> ObtenerDisponibles()
        {
            var lista = new List<Publicacion>();
            try
            {
                using (var con = _conexion.ObtenerConexion())
                {
                    string sql = $@"{SelectBase}
                                    WHERE pub.activo = TRUE AND pub.stock > 0
                                    ORDER BY pub.id DESC";
                    using (var cmd = new NpgsqlCommand(sql, con))
                    {
                        con.Open();
                        using (var r = cmd.ExecuteReader())
                            while (r.Read()) lista.Add(Mapear(r));
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Error BD al listar publicaciones: {ex.Message}", ex);
            }
            return lista;
        }

        public List<Publicacion> Buscar(string texto)
        {
            var lista = new List<Publicacion>();
            try
            {
                using (var con = _conexion.ObtenerConexion())
                {
                    string sql = $@"{SelectBase}
                                    WHERE pub.activo = TRUE AND pub.stock > 0
                                      AND pr.titulo ILIKE @Texto
                                    ORDER BY pub.id DESC";
                    using (var cmd = new NpgsqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@Texto", "%" + texto + "%");
                        con.Open();
                        using (var r = cmd.ExecuteReader())
                            while (r.Read()) lista.Add(Mapear(r));
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Error BD al buscar publicaciones: {ex.Message}", ex);
            }
            return lista;
        }

        public List<Publicacion> ObtenerPorVendedor(int vendedorId)
        {
            var lista = new List<Publicacion>();
            try
            {
                using (var con = _conexion.ObtenerConexion())
                {
                    string sql = $@"{SelectBase}
                                    WHERE pr.propietario_id = @VendedorId AND pub.activo = TRUE
                                    ORDER BY pub.id DESC";
                    using (var cmd = new NpgsqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@VendedorId", vendedorId);
                        con.Open();
                        using (var r = cmd.ExecuteReader())
                            while (r.Read()) lista.Add(Mapear(r));
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Error BD al listar publicaciones del vendedor: {ex.Message}", ex);
            }
            return lista;
        }

        public Publicacion ObtenerPorId(int id)
        {
            try
            {
                using (var con = _conexion.ObtenerConexion())
                {
                    string sql = $"{SelectBase} WHERE pub.id = @Id";
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
                throw new Exception($"Error BD al buscar publicación: {ex.Message}", ex);
            }
            return null;
        }

        public int Insertar(Publicacion publicacion)
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
                            // 1. Insertar producto base
                            string sqlProd = @"INSERT INTO public.productos (propietario_id, titulo, descripcion)
                                               VALUES (@VendedorId, @Titulo, @Descripcion)
                                               RETURNING id";
                            int productoId;
                            using (var cmdProd = new NpgsqlCommand(sqlProd, con, tx))
                            {
                                cmdProd.Parameters.AddWithValue("@VendedorId", publicacion.VendedorId);
                                cmdProd.Parameters.AddWithValue("@Titulo", publicacion.Titulo);
                                cmdProd.Parameters.AddWithValue("@Descripcion", (object)publicacion.Descripcion ?? DBNull.Value);
                                productoId = Convert.ToInt32(cmdProd.ExecuteScalar());
                            }

                            // 2. Insertar foto si existe
                            if (publicacion.Imagen != null && publicacion.Imagen.Length > 0)
                            {
                                string sqlFoto = @"INSERT INTO public.fotos_productos (id_producto, imagen)
                                                   VALUES (@ProductoId, @Imagen)";
                                using (var cmdFoto = new NpgsqlCommand(sqlFoto, con, tx))
                                {
                                    cmdFoto.Parameters.AddWithValue("@ProductoId", productoId);
                                    cmdFoto.Parameters.Add(new NpgsqlParameter("@Imagen", NpgsqlDbType.Bytea) { Value = publicacion.Imagen });
                                    cmdFoto.ExecuteNonQuery();
                                }
                            }

                            // 3. Insertar publicación padre
                            string sqlPub = @"INSERT INTO public.publicaciones (producto_id, stock, modalidad)
                                              VALUES (@ProductoId, @Stock, @Modalidad)
                                              RETURNING id";
                            int publicacionId;
                            using (var cmdPub = new NpgsqlCommand(sqlPub, con, tx))
                            {
                                cmdPub.Parameters.AddWithValue("@ProductoId", productoId);
                                cmdPub.Parameters.AddWithValue("@Stock", publicacion.Stock);
                                cmdPub.Parameters.AddWithValue("@Modalidad", publicacion.Modalidad.ToString());
                                publicacionId = Convert.ToInt32(cmdPub.ExecuteScalar());
                            }

                            // 4. Insertar subtipo Venta si aplica
                            if (publicacion.AceptaVenta)
                            {
                                string sqlVenta = @"INSERT INTO public.publicaciones_venta (id_publicacion, precio)
                                                    VALUES (@IdPub, @Precio)";
                                using (var cmdVenta = new NpgsqlCommand(sqlVenta, con, tx))
                                {
                                    cmdVenta.Parameters.AddWithValue("@IdPub", publicacionId);
                                    cmdVenta.Parameters.AddWithValue("@Precio", publicacion.Precio);
                                    cmdVenta.ExecuteNonQuery();
                                }
                            }

                            // 5. Insertar subtipo Trueque si aplica
                            if (publicacion.AceptaTrueque)
                            {
                                string sqlTrueque = @"INSERT INTO public.publicaciones_trueque (id_publicacion, trueque_busca)
                                                      VALUES (@IdPub, @TruequeBusca)";
                                using (var cmdTrueque = new NpgsqlCommand(sqlTrueque, con, tx))
                                {
                                    cmdTrueque.Parameters.AddWithValue("@IdPub", publicacionId);
                                    cmdTrueque.Parameters.AddWithValue("@TruequeBusca", publicacion.TruequeBusca);
                                    cmdTrueque.ExecuteNonQuery();
                                }
                            }

                            tx.Commit();
                            publicacion.ProductoId = productoId;
                            publicacion.Id = publicacionId;
                            return publicacionId;
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
                throw new Exception($"Error BD al insertar publicación: {ex.Message}", ex);
            }
        }

        public void Actualizar(Publicacion publicacion)
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
                            // 1. Actualizar producto
                            string sqlProd = @"UPDATE public.productos
                                               SET titulo = @Titulo, descripcion = @Descripcion
                                               WHERE id = (SELECT producto_id FROM public.publicaciones WHERE id = @Id)
                                               RETURNING id";
                            int productoId;
                            using (var cmdProd = new NpgsqlCommand(sqlProd, con, tx))
                            {
                                cmdProd.Parameters.AddWithValue("@Id", publicacion.Id);
                                cmdProd.Parameters.AddWithValue("@Titulo", publicacion.Titulo);
                                cmdProd.Parameters.AddWithValue("@Descripcion", (object)publicacion.Descripcion ?? DBNull.Value);
                                productoId = Convert.ToInt32(cmdProd.ExecuteScalar());
                            }

                            // 2. Sincronizar foto
                            if (publicacion.Imagen != null && publicacion.Imagen.Length > 0)
                            {
                                string sqlUpdFoto = @"UPDATE public.fotos_productos
                                                      SET imagen = @Imagen
                                                      WHERE id = (SELECT id FROM public.fotos_productos WHERE id_producto = @ProductoId ORDER BY id ASC LIMIT 1)";
                                int filas;
                                using (var cmdUpdFoto = new NpgsqlCommand(sqlUpdFoto, con, tx))
                                {
                                    cmdUpdFoto.Parameters.AddWithValue("@ProductoId", productoId);
                                    cmdUpdFoto.Parameters.Add(new NpgsqlParameter("@Imagen", NpgsqlDbType.Bytea) { Value = publicacion.Imagen });
                                    filas = cmdUpdFoto.ExecuteNonQuery();
                                }

                                if (filas == 0)
                                {
                                    string sqlInsFoto = "INSERT INTO public.fotos_productos (id_producto, imagen) VALUES (@ProductoId, @Imagen)";
                                    using (var cmdInsFoto = new NpgsqlCommand(sqlInsFoto, con, tx))
                                    {
                                        cmdInsFoto.Parameters.AddWithValue("@ProductoId", productoId);
                                        cmdInsFoto.Parameters.Add(new NpgsqlParameter("@Imagen", NpgsqlDbType.Bytea) { Value = publicacion.Imagen });
                                        cmdInsFoto.ExecuteNonQuery();
                                    }
                                }
                            }
                            else
                            {
                                using (var cmdDelFoto = new NpgsqlCommand("DELETE FROM public.fotos_productos WHERE id_producto = @ProductoId", con, tx))
                                {
                                    cmdDelFoto.Parameters.AddWithValue("@ProductoId", productoId);
                                    cmdDelFoto.ExecuteNonQuery();
                                }
                            }

                            // 3. Actualizar publicación padre
                            string sqlPub = @"UPDATE public.publicaciones
                                              SET stock = @Stock, modalidad = @Modalidad
                                              WHERE id = @Id";
                            using (var cmdPub = new NpgsqlCommand(sqlPub, con, tx))
                            {
                                cmdPub.Parameters.AddWithValue("@Id", publicacion.Id);
                                cmdPub.Parameters.AddWithValue("@Stock", publicacion.Stock);
                                cmdPub.Parameters.AddWithValue("@Modalidad", publicacion.Modalidad.ToString());
                                cmdPub.ExecuteNonQuery();
                            }

                            // 4. Sincronizar subtipo Venta
                            if (publicacion.AceptaVenta)
                            {
                                string sqlVenta = @"INSERT INTO public.publicaciones_venta (id_publicacion, precio)
                                                    VALUES (@IdPub, @Precio)
                                                    ON CONFLICT (id_publicacion) DO UPDATE SET precio = EXCLUDED.precio";
                                using (var cmdVenta = new NpgsqlCommand(sqlVenta, con, tx))
                                {
                                    cmdVenta.Parameters.AddWithValue("@IdPub", publicacion.Id);
                                    cmdVenta.Parameters.AddWithValue("@Precio", publicacion.Precio);
                                    cmdVenta.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                using (var cmdDelVenta = new NpgsqlCommand("DELETE FROM public.publicaciones_venta WHERE id_publicacion = @IdPub", con, tx))
                                {
                                    cmdDelVenta.Parameters.AddWithValue("@IdPub", publicacion.Id);
                                    cmdDelVenta.ExecuteNonQuery();
                                }
                            }

                            // 5. Sincronizar subtipo Trueque
                            if (publicacion.AceptaTrueque)
                            {
                                string sqlTrueque = @"INSERT INTO public.publicaciones_trueque (id_publicacion, trueque_busca)
                                                      VALUES (@IdPub, @TruequeBusca)
                                                      ON CONFLICT (id_publicacion) DO UPDATE SET trueque_busca = EXCLUDED.trueque_busca";
                                using (var cmdTrueque = new NpgsqlCommand(sqlTrueque, con, tx))
                                {
                                    cmdTrueque.Parameters.AddWithValue("@IdPub", publicacion.Id);
                                    cmdTrueque.Parameters.AddWithValue("@TruequeBusca", publicacion.TruequeBusca);
                                    cmdTrueque.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                using (var cmdDelTrueque = new NpgsqlCommand("DELETE FROM public.publicaciones_trueque WHERE id_publicacion = @IdPub", con, tx))
                                {
                                    cmdDelTrueque.Parameters.AddWithValue("@IdPub", publicacion.Id);
                                    cmdDelTrueque.ExecuteNonQuery();
                                }
                            }

                            tx.Commit();
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
                throw new Exception($"Error BD al actualizar publicación: {ex.Message}", ex);
            }
        }

        public void Eliminar(int id)
        {
            try
            {
                using (var con = _conexion.ObtenerConexion())
                {
                    string sql = "UPDATE public.publicaciones SET activo = FALSE WHERE id = @Id";
                    using (var cmd = new NpgsqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Error BD al eliminar publicación: {ex.Message}", ex);
            }
        }

        private Publicacion Mapear(NpgsqlDataReader r) => new Publicacion
        {
            Id = Convert.ToInt32(r["id"]),
            ProductoId = Convert.ToInt32(r["producto_id"]),
            VendedorId = Convert.ToInt32(r["propietario_id"]),
            Titulo = r["titulo"].ToString(),
            Descripcion = r.IsDBNull(r.GetOrdinal("descripcion"))
                            ? null
                            : r["descripcion"].ToString(),
            Precio = r.IsDBNull(r.GetOrdinal("precio")) ? 0m : Convert.ToDecimal(r["precio"]),
            Stock = Convert.ToInt32(r["stock"]),
            Activo = Convert.ToBoolean(r["activo"]),
            Imagen = r.IsDBNull(r.GetOrdinal("imagen"))
                ? null
                : (byte[])r["imagen"],
            Modalidad = (ModalidadPublicacion)Enum.Parse(typeof(ModalidadPublicacion), r["modalidad"].ToString()),
            TruequeBusca = r.IsDBNull(r.GetOrdinal("trueque_busca"))
                ? null
                : r["trueque_busca"].ToString()
        };
    }
}