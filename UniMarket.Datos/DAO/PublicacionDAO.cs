using System;
using System.Collections.Generic;
using Npgsql;
using NpgsqlTypes;
using UniMarket.Entidades;

namespace UniMarket.Datos.DAO
{
    public class PublicacionDAO : IPublicacionDAO
    {
        // precio y trueque_busca vienen de sus tablas (LEFT JOIN: pueden no existir);
        // la imagen es la primera foto del producto
        private const string SelectBase = @"
            SELECT pub.id, pub.producto_id, pr.propietario_id, pr.titulo, pr.descripcion,
                   (SELECT f.imagen FROM public.fotos_productos f
                     WHERE f.id_producto = pr.id ORDER BY f.id LIMIT 1) AS imagen,
                   v.precio, pub.stock, pub.activo, pub.modalidad, t.trueque_busca
            FROM public.publicaciones pub
            INNER JOIN public.productos pr ON pub.producto_id = pr.id
            LEFT JOIN public.publicaciones_venta v ON v.id_publicacion = pub.id
            LEFT JOIN public.publicaciones_trueque t ON t.id_publicacion = pub.id";

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
                            // 1. El producto (el objeto)
                            string sqlProd = @"INSERT INTO public.productos (propietario_id, titulo, descripcion)
                                               VALUES (@VendedorId, @Titulo, @Descripcion)
                                               RETURNING id";
                            int productoId;
                            using (var cmd = new NpgsqlCommand(sqlProd, con, tx))
                            {
                                cmd.Parameters.AddWithValue("@VendedorId", publicacion.VendedorId);
                                AgregarParamsProducto(cmd, publicacion);
                                productoId = Convert.ToInt32(cmd.ExecuteScalar());
                            }

                            // 2. Su foto
                            GuardarFoto(con, tx, productoId, publicacion.Imagen);

                            // 3. La publicación (cómo se ofrece)
                            string sqlPub = @"INSERT INTO public.publicaciones (producto_id, stock, modalidad)
                                              VALUES (@ProductoId, @Stock, @Modalidad)
                                              RETURNING id";
                            int publicacionId;
                            using (var cmd = new NpgsqlCommand(sqlPub, con, tx))
                            {
                                cmd.Parameters.AddWithValue("@ProductoId", productoId);
                                cmd.Parameters.AddWithValue("@Stock", publicacion.Stock);
                                cmd.Parameters.AddWithValue("@Modalidad", publicacion.Modalidad.ToString());
                                publicacionId = Convert.ToInt32(cmd.ExecuteScalar());
                            }

                            // 4. Sus condiciones de venta y/o de trueque
                            GuardarCondiciones(con, tx, publicacionId, publicacion);

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
                            // Producto al que pertenece esta publicación
                            int productoId;
                            using (var cmd = new NpgsqlCommand(
                                "SELECT producto_id FROM public.publicaciones WHERE id = @Id", con, tx))
                            {
                                cmd.Parameters.AddWithValue("@Id", publicacion.Id);
                                object resultado = cmd.ExecuteScalar();
                                if (resultado == null)
                                    throw new Exception("La publicación no existe.");
                                productoId = Convert.ToInt32(resultado);
                            }

                            // 1. Datos del producto
                            string sqlProd = @"UPDATE public.productos
                                               SET titulo = @Titulo, descripcion = @Descripcion
                                               WHERE id = @ProductoId";
                            using (var cmd = new NpgsqlCommand(sqlProd, con, tx))
                            {
                                cmd.Parameters.AddWithValue("@ProductoId", productoId);
                                AgregarParamsProducto(cmd, publicacion);
                                cmd.ExecuteNonQuery();
                            }

                            // 2. Foto
                            GuardarFoto(con, tx, productoId, publicacion.Imagen);

                            // 3. Datos de la publicación
                            string sqlPub = @"UPDATE public.publicaciones
                                              SET stock = @Stock, modalidad = @Modalidad
                                              WHERE id = @Id";
                            using (var cmd = new NpgsqlCommand(sqlPub, con, tx))
                            {
                                cmd.Parameters.AddWithValue("@Id", publicacion.Id);
                                cmd.Parameters.AddWithValue("@Stock", publicacion.Stock);
                                cmd.Parameters.AddWithValue("@Modalidad", publicacion.Modalidad.ToString());
                                cmd.ExecuteNonQuery();
                            }

                            // 4. Condiciones (se crean o se borran según la modalidad nueva)
                            GuardarCondiciones(con, tx, publicacion.Id, publicacion);

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

        // ----- Apoyo -----

        // Crea o actualiza la fila de venta y/o de trueque; borra la que ya no aplique
        private void GuardarCondiciones(NpgsqlConnection con, NpgsqlTransaction tx,
                                        int publicacionId, Publicacion publicacion)
        {
            if (publicacion.AceptaVenta)
            {
                string sql = @"INSERT INTO public.publicaciones_venta (id_publicacion, precio)
                               VALUES (@Id, @Precio)
                               ON CONFLICT (id_publicacion) DO UPDATE SET precio = EXCLUDED.precio";
                using (var cmd = new NpgsqlCommand(sql, con, tx))
                {
                    cmd.Parameters.AddWithValue("@Id", publicacionId);
                    cmd.Parameters.AddWithValue("@Precio", publicacion.Precio);
                    cmd.ExecuteNonQuery();
                }
            }
            else
            {
                using (var cmd = new NpgsqlCommand(
                    "DELETE FROM public.publicaciones_venta WHERE id_publicacion = @Id", con, tx))
                {
                    cmd.Parameters.AddWithValue("@Id", publicacionId);
                    cmd.ExecuteNonQuery();
                }
            }

            if (publicacion.AceptaTrueque)
            {
                string sql = @"INSERT INTO public.publicaciones_trueque (id_publicacion, trueque_busca)
                               VALUES (@Id, @Busca)
                               ON CONFLICT (id_publicacion) DO UPDATE SET trueque_busca = EXCLUDED.trueque_busca";
                using (var cmd = new NpgsqlCommand(sql, con, tx))
                {
                    cmd.Parameters.AddWithValue("@Id", publicacionId);
                    cmd.Parameters.AddWithValue("@Busca", publicacion.TruequeBusca);
                    cmd.ExecuteNonQuery();
                }
            }
            else
            {
                using (var cmd = new NpgsqlCommand(
                    "DELETE FROM public.publicaciones_trueque WHERE id_publicacion = @Id", con, tx))
                {
                    cmd.Parameters.AddWithValue("@Id", publicacionId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Reemplaza la primera foto del producto, o la crea si no tenía
        private void GuardarFoto(NpgsqlConnection con, NpgsqlTransaction tx, int productoId, byte[] imagen)
        {
            if (imagen == null || imagen.Length == 0) return;

            string sqlUpdate = @"UPDATE public.fotos_productos SET imagen = @Imagen
                                 WHERE id = (SELECT id FROM public.fotos_productos
                                             WHERE id_producto = @ProductoId ORDER BY id LIMIT 1)";
            using (var cmd = new NpgsqlCommand(sqlUpdate, con, tx))
            {
                cmd.Parameters.AddWithValue("@ProductoId", productoId);
                cmd.Parameters.Add(new NpgsqlParameter("@Imagen", NpgsqlDbType.Bytea) { Value = imagen });
                if (cmd.ExecuteNonQuery() > 0) return;
            }

            string sqlInsert = @"INSERT INTO public.fotos_productos (id_producto, imagen)
                                 VALUES (@ProductoId, @Imagen)";
            using (var cmd = new NpgsqlCommand(sqlInsert, con, tx))
            {
                cmd.Parameters.AddWithValue("@ProductoId", productoId);
                cmd.Parameters.Add(new NpgsqlParameter("@Imagen", NpgsqlDbType.Bytea) { Value = imagen });
                cmd.ExecuteNonQuery();
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

        private void AgregarParamsProducto(NpgsqlCommand cmd, Publicacion publicacion)
        {
            cmd.Parameters.AddWithValue("@Titulo", publicacion.Titulo);
            cmd.Parameters.AddWithValue("@Descripcion", (object)publicacion.Descripcion ?? DBNull.Value);
        }
    }
}