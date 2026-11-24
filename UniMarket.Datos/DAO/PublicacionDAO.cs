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
                   pr.imagen, pub.precio, pub.stock, pub.activo, pub.modalidad, pub.trueque_busca
            FROM public.publicaciones pub
            INNER JOIN public.productos pr ON pub.producto_id = pr.id";

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
                            // 1. Insertar el producto físico
                            string sqlProd = @"INSERT INTO public.productos (propietario_id, titulo, descripcion, imagen)
                                               VALUES (@VendedorId, @Titulo, @Descripcion, @Imagen)
                                               RETURNING id";
                            int productoId;
                            using (var cmdProd = new NpgsqlCommand(sqlProd, con, tx))
                            {
                                cmdProd.Parameters.AddWithValue("@VendedorId", publicacion.VendedorId);
                                AgregarParamsProducto(cmdProd, publicacion);
                                productoId = Convert.ToInt32(cmdProd.ExecuteScalar());
                            }

                            // 2. Insertar la publicación comercial asociada
                            string sqlPub = @"INSERT INTO public.publicaciones (producto_id, precio, stock, modalidad, trueque_busca)
                                              VALUES (@ProductoId, @Precio, @Stock, @Modalidad, @TruequeBusca)
                                              RETURNING id";
                            int publicacionId;
                            using (var cmdPub = new NpgsqlCommand(sqlPub, con, tx))
                            {
                                cmdPub.Parameters.AddWithValue("@ProductoId", productoId);
                                AgregarParamsPublicacion(cmdPub, publicacion);
                                publicacionId = Convert.ToInt32(cmdPub.ExecuteScalar());
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
                            // 1. Actualizar datos del producto físico vinculado
                            string sqlProd = @"UPDATE public.productos
                                               SET titulo = @Titulo, descripcion = @Descripcion, imagen = @Imagen
                                               WHERE id = (SELECT producto_id FROM public.publicaciones WHERE id = @Id)";
                            using (var cmdProd = new NpgsqlCommand(sqlProd, con, tx))
                            {
                                cmdProd.Parameters.AddWithValue("@Id", publicacion.Id);
                                AgregarParamsProducto(cmdProd, publicacion);
                                cmdProd.ExecuteNonQuery();
                            }

                            // 2. Actualizar condiciones comerciales de la publicación
                            string sqlPub = @"UPDATE public.publicaciones
                                              SET precio = @Precio, stock = @Stock,
                                                  modalidad = @Modalidad, trueque_busca = @TruequeBusca
                                              WHERE id = @Id";
                            using (var cmdPub = new NpgsqlCommand(sqlPub, con, tx))
                            {
                                cmdPub.Parameters.AddWithValue("@Id", publicacion.Id);
                                AgregarParamsPublicacion(cmdPub, publicacion);
                                cmdPub.ExecuteNonQuery();
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

        private void AgregarParamsProducto(NpgsqlCommand cmd, Publicacion publicacion)
        {
            cmd.Parameters.AddWithValue("@Titulo", publicacion.Titulo);
            cmd.Parameters.AddWithValue("@Descripcion", (object)publicacion.Descripcion ?? DBNull.Value);

            var paramImagen = new NpgsqlParameter("@Imagen", NpgsqlDbType.Bytea);
            paramImagen.Value = (object)publicacion.Imagen ?? DBNull.Value;
            cmd.Parameters.Add(paramImagen);
        }

        private void AgregarParamsPublicacion(NpgsqlCommand cmd, Publicacion publicacion)
        {
            cmd.Parameters.AddWithValue("@Precio", publicacion.AceptaVenta ? (object)publicacion.Precio : DBNull.Value);
            cmd.Parameters.AddWithValue("@Stock", publicacion.Stock);
            cmd.Parameters.AddWithValue("@Modalidad", publicacion.Modalidad.ToString());
            cmd.Parameters.AddWithValue("@TruequeBusca", publicacion.AceptaTrueque ? (object)publicacion.TruequeBusca : DBNull.Value);
        }
    }
}