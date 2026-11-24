using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using UniMarket.Entidades;

namespace UniMarket.Datos.DAO
{
    public class UsuarioDAO : IUsuarioDAO
    {
        private const string SelectBase = @"
            SELECT u.id, u.correo, u.contrasena, u.activo, u.fecha_registro,
                   p.nombre, p.telefono, p.foto
            FROM public.usuarios u
            INNER JOIN public.perfiles_usuarios p ON p.id_usuario = u.id";

        private readonly Conexion _conexion;

        public UsuarioDAO()
        {
            _conexion = new Conexion();
        }

        public int Insertar(Usuario usuario)
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
                            // 1. Insertar credenciales en 'usuarios'
                            string sqlUsuario = @"INSERT INTO public.usuarios (correo, contrasena)
                                                  VALUES (@Correo, @Contrasena)
                                                  RETURNING id";
                            int usuarioId;
                            using (var cmdUser = new NpgsqlCommand(sqlUsuario, con, tx))
                            {
                                cmdUser.Parameters.AddWithValue("@Correo", usuario.Correo);
                                cmdUser.Parameters.AddWithValue("@Contrasena", usuario.Contrasena);
                                usuarioId = Convert.ToInt32(cmdUser.ExecuteScalar());
                            }

                            // 2. Insertar datos personales en 'perfiles_usuarios'
                            string sqlPerfil = @"INSERT INTO public.perfiles_usuarios (id_usuario, nombre, telefono, foto)
                                                 VALUES (@IdUsuario, @Nombre, @Telefono, @Foto)";
                            using (var cmdPerfil = new NpgsqlCommand(sqlPerfil, con, tx))
                            {
                                cmdPerfil.Parameters.AddWithValue("@IdUsuario", usuarioId);
                                cmdPerfil.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                                cmdPerfil.Parameters.AddWithValue("@Telefono", usuario.Telefono);

                                var paramFoto = new NpgsqlParameter("@Foto", NpgsqlDbType.Bytea);
                                paramFoto.Value = (object)usuario.Foto ?? DBNull.Value;
                                cmdPerfil.Parameters.Add(paramFoto);

                                cmdPerfil.ExecuteNonQuery();
                            }

                            tx.Commit();
                            usuario.Id = usuarioId;
                            return usuarioId;
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
                throw new Exception($"Error BD al insertar usuario: {ex.Message}", ex);
            }
        }

        public void Actualizar(Usuario usuario)
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
                            string sqlUsuario = @"UPDATE public.usuarios
                                                  SET correo = @Correo, contrasena = @Contrasena
                                                  WHERE id = @Id";
                            using (var cmdUser = new NpgsqlCommand(sqlUsuario, con, tx))
                            {
                                cmdUser.Parameters.AddWithValue("@Id", usuario.Id);
                                cmdUser.Parameters.AddWithValue("@Correo", usuario.Correo);
                                cmdUser.Parameters.AddWithValue("@Contrasena", usuario.Contrasena);
                                cmdUser.ExecuteNonQuery();
                            }

                            string sqlPerfil = @"UPDATE public.perfiles_usuarios
                                                 SET nombre = @Nombre, telefono = @Telefono
                                                 WHERE id_usuario = @Id";
                            using (var cmdPerfil = new NpgsqlCommand(sqlPerfil, con, tx))
                            {
                                cmdPerfil.Parameters.AddWithValue("@Id", usuario.Id);
                                cmdPerfil.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                                cmdPerfil.Parameters.AddWithValue("@Telefono", usuario.Telefono);
                                cmdPerfil.ExecuteNonQuery();
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
                throw new Exception($"Error BD al actualizar usuario: {ex.Message}", ex);
            }
        }

        public void ActualizarPerfil(Usuario usuario)
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
                            string sqlUsuario = @"UPDATE public.usuarios
                                                  SET correo = @Correo
                                                  WHERE id = @Id";
                            using (var cmdUser = new NpgsqlCommand(sqlUsuario, con, tx))
                            {
                                cmdUser.Parameters.AddWithValue("@Id", usuario.Id);
                                cmdUser.Parameters.AddWithValue("@Correo", usuario.Correo);
                                cmdUser.ExecuteNonQuery();
                            }

                            string sqlPerfil = @"UPDATE public.perfiles_usuarios
                                                 SET nombre = @Nombre, telefono = @Telefono, foto = @Foto
                                                 WHERE id_usuario = @Id";
                            using (var cmdPerfil = new NpgsqlCommand(sqlPerfil, con, tx))
                            {
                                cmdPerfil.Parameters.AddWithValue("@Id", usuario.Id);
                                cmdPerfil.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                                cmdPerfil.Parameters.AddWithValue("@Telefono", usuario.Telefono);

                                var paramFoto = new NpgsqlParameter("@Foto", NpgsqlDbType.Bytea);
                                paramFoto.Value = (object)usuario.Foto ?? DBNull.Value;
                                cmdPerfil.Parameters.Add(paramFoto);

                                cmdPerfil.ExecuteNonQuery();
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
                throw new Exception($"Error BD al actualizar el perfil: {ex.Message}", ex);
            }
        }

        public void ActualizarContrasena(int id, string contrasenaHash)
        {
            try
            {
                using (var con = _conexion.ObtenerConexion())
                {
                    string sql = "UPDATE public.usuarios SET contrasena = @Contrasena WHERE id = @Id";
                    using (var cmd = new NpgsqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.Parameters.AddWithValue("@Contrasena", contrasenaHash);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Error BD al cambiar la contraseña: {ex.Message}", ex);
            }
        }

        public void Eliminar(int id)
        {
            try
            {
                using (var con = _conexion.ObtenerConexion())
                {
                    string sql = "UPDATE public.usuarios SET activo = FALSE WHERE id = @Id";
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
                throw new Exception($"Error BD al eliminar usuario: {ex.Message}", ex);
            }
        }

        public Usuario ObtenerPorCorreo(string correo)
        {
            try
            {
                using (var con = _conexion.ObtenerConexion())
                {
                    string sql = $"{SelectBase} WHERE u.correo = @Correo";
                    using (var cmd = new NpgsqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@Correo", correo);
                        con.Open();
                        using (var r = cmd.ExecuteReader())
                            if (r.Read()) return Mapear(r);
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Error BD al buscar por correo: {ex.Message}", ex);
            }
            return null;
        }

        public Usuario ObtenerPorId(int id)
        {
            try
            {
                using (var con = _conexion.ObtenerConexion())
                {
                    string sql = $"{SelectBase} WHERE u.id = @Id";
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
                throw new Exception($"Error BD al buscar usuario: {ex.Message}", ex);
            }
            return null;
        }

        public List<Usuario> ObtenerTodos()
        {
            var lista = new List<Usuario>();
            try
            {
                using (var con = _conexion.ObtenerConexion())
                {
                    using (var cmd = new NpgsqlCommand(SelectBase, con))
                    {
                        con.Open();
                        using (var r = cmd.ExecuteReader())
                            while (r.Read()) lista.Add(Mapear(r));
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Error BD al listar usuarios: {ex.Message}", ex);
            }
            return lista;
        }

        public int ContarPublicaciones(int usuarioId)
        {
            string sql = @"SELECT COUNT(*) 
                           FROM public.publicaciones pub
                           INNER JOIN public.productos pr ON pub.producto_id = pr.id
                           WHERE pr.propietario_id = @Id AND pub.activo = TRUE";
            return ContarPorUsuario(sql, usuarioId, "las publicaciones");
        }

        public int ContarCompras(int usuarioId)
        {
            string sql = @"SELECT COUNT(*) FROM public.solicitudes
                           WHERE solicitante_id = @Id AND estado = 'Aceptada' AND tipo = 'Compra'";
            return ContarPorUsuario(sql, usuarioId, "las compras");
        }

        public int ContarTrueques(int usuarioId)
        {
            string sql = @"SELECT COUNT(*)
                           FROM public.solicitudes s
                           INNER JOIN public.publicaciones pub ON pub.id = s.publicacion_id
                           INNER JOIN public.productos pr ON pr.id = pub.producto_id
                           WHERE s.estado = 'Aceptada' AND s.tipo = 'Trueque'
                             AND (s.solicitante_id = @Id OR pr.propietario_id = @Id)";
            return ContarPorUsuario(sql, usuarioId, "los trueques");
        }

        private int ContarPorUsuario(string sql, int usuarioId, string queCuenta)
        {
            try
            {
                using (var con = _conexion.ObtenerConexion())
                using (var cmd = new NpgsqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Id", usuarioId);
                    con.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Error BD al contar {queCuenta}: {ex.Message}", ex);
            }
        }

        public void DesactivarCuenta(int usuarioId)
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
                            string sqlSolicitudes = @"UPDATE public.solicitudes
                                                      SET estado = 'Rechazada'
                                                      WHERE estado = 'Pendiente'
                                                        AND (solicitante_id = @Id
                                                             OR publicacion_id IN
                                                                (SELECT pub.id FROM public.publicaciones pub
                                                                 INNER JOIN public.productos pr ON pub.producto_id = pr.id
                                                                 WHERE pr.propietario_id = @Id))";
                            using (var cmd = new NpgsqlCommand(sqlSolicitudes, con, tx))
                            {
                                cmd.Parameters.AddWithValue("@Id", usuarioId);
                                cmd.ExecuteNonQuery();
                            }

                            string sqlPublicaciones = @"UPDATE public.publicaciones
                                                        SET activo = false
                                                        WHERE producto_id IN
                                                           (SELECT id FROM public.productos WHERE propietario_id = @Id)";
                            using (var cmd = new NpgsqlCommand(sqlPublicaciones, con, tx))
                            {
                                cmd.Parameters.AddWithValue("@Id", usuarioId);
                                cmd.ExecuteNonQuery();
                            }

                            string sqlUsuario = @"UPDATE public.usuarios
                                                  SET activo = false
                                                  WHERE id = @Id AND activo = true";
                            using (var cmd = new NpgsqlCommand(sqlUsuario, con, tx))
                            {
                                cmd.Parameters.AddWithValue("@Id", usuarioId);
                                if (cmd.ExecuteNonQuery() != 1)
                                    throw new Exception("La cuenta no existe o ya estaba desactivada.");
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
                throw new Exception($"Error BD al desactivar la cuenta: {ex.Message}", ex);
            }
        }

        private Usuario Mapear(NpgsqlDataReader r) => new Usuario
        {
            Id = Convert.ToInt32(r["id"]),
            Correo = r["correo"].ToString(),
            Contrasena = r["contrasena"].ToString(),
            Activo = Convert.ToBoolean(r["activo"]),
            FechaRegistro = Convert.ToDateTime(r["fecha_registro"]).ToLocalTime(),
            Nombre = r["nombre"].ToString(),
            Telefono = r["telefono"].ToString(),
            Foto = r.IsDBNull(r.GetOrdinal("foto")) ? null : (byte[])r["foto"]
        };
    }
}