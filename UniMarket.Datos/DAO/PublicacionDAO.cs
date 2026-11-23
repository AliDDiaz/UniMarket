using System;
using System.Collections.Generic;
using Npgsql;
using NpgsqlTypes;
using UniMarket.Entidades;

namespace UniMarket.Datos.DAO
{
    public class PublicacionDAO : IPublicacionDAO
    {
        private const string Columnas =
            "id, vendedor_id, titulo, descripcion, precio, stock, activo, imagen, modalidad, trueque_busca";

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
                    string sql = $@"SELECT {Columnas} FROM publicaciones
                                    WHERE activo = TRUE AND stock > 0
                                    ORDER BY id DESC";
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
                    string sql = $@"SELECT {Columnas} FROM publicaciones
                            WHERE activo = TRUE AND stock > 0
                              AND titulo ILIKE @Texto
                            ORDER BY id DESC";
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
                    string sql = $@"SELECT {Columnas} FROM publicaciones
                                    WHERE vendedor_id = @VendedorId AND activo = TRUE
                                    ORDER BY id DESC";
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
                    string sql = $"SELECT {Columnas} FROM publicaciones WHERE id = @Id";
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
                    string sql = @"INSERT INTO publicaciones (vendedor_id, titulo, descripcion, precio, stock, imagen, modalidad, trueque_busca)
                                   VALUES (@VendedorId, @Titulo, @Descripcion, @Precio, @Stock, @Imagen, @Modalidad, @TruequeBusca)
                                   RETURNING id";
                    using (var cmd = new NpgsqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@VendedorId", publicacion.VendedorId);
                        AgregarParams(cmd, publicacion);
                        con.Open();
                        return Convert.ToInt32(cmd.ExecuteScalar());
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
                    string sql = @"UPDATE publicaciones
                                   SET titulo = @Titulo, descripcion = @Descripcion,
                                       precio = @Precio, stock = @Stock, imagen = @Imagen,
                                       modalidad = @Modalidad, trueque_busca = @TruequeBusca
                                   WHERE id = @Id";
                    using (var cmd = new NpgsqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@Id", publicacion.Id);
                        AgregarParams(cmd, publicacion);
                        con.Open();
                        cmd.ExecuteNonQuery();
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
                    string sql = "UPDATE publicaciones SET activo = FALSE WHERE id = @Id";
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
            VendedorId = Convert.ToInt32(r["vendedor_id"]),
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

        private void AgregarParams(NpgsqlCommand cmd, Publicacion publicacion)
        {
            cmd.Parameters.AddWithValue("@Titulo", publicacion.Titulo);
            cmd.Parameters.AddWithValue("@Descripcion", (object)publicacion.Descripcion ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Precio", publicacion.AceptaVenta ? (object)publicacion.Precio : DBNull.Value);
            cmd.Parameters.AddWithValue("@Stock", publicacion.Stock);
            cmd.Parameters.AddWithValue("@Modalidad", publicacion.Modalidad.ToString());
            cmd.Parameters.AddWithValue("@TruequeBusca", (object)publicacion.TruequeBusca ?? DBNull.Value);

            var paramImagen = new NpgsqlParameter("@Imagen", NpgsqlDbType.Bytea);
            paramImagen.Value = (object)publicacion.Imagen ?? DBNull.Value;
            cmd.Parameters.Add(paramImagen);
        }
    }
}