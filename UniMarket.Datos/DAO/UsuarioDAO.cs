using Microsoft.Extensions.Logging.Abstractions;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NpgsqlTypes;
using UniMarket.Entidades;

namespace UniMarket.Datos.DAO
{
    public class UsuarioDAO : IUsuarioDAO
    {
        private const string Columnas = "id, nombre, correo, telefono, contrasena, rol, activo, fecha_registro, foto";
        private readonly Conexion _conexion;

        public UsuarioDAO()
        {

            _conexion = new Conexion();

        }

        public void Actualizar(Usuario usuario)
        {

            try
            {

                using (var con = _conexion.ObtenerConexion())
                {

                    string sql = @"UPDATE usuarios SET nombre = @Nombre, correo = @Correo, telefono = @Telefono,
                                 contrasena = @Contrasena, rol = @Rol WHERE id = @Id";
                    using(var cmd = new NpgsqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@Id", usuario.Id);
                        AgregarParams(cmd, usuario);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

            }
            catch(NpgsqlException ex)
            {

                throw new Exception($"Error BD al actualizar usuario: {ex.Message}", ex);

            }

        }

        public void Eliminar(int id)
        {
            try
            {

                using (var con = _conexion.ObtenerConexion())
                {

                    string sql = "UPDATE usuarios SET activo = FALSE WHERE id = @Id";
                    using (var cmd = new NpgsqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }

                }
            }
            catch(NpgsqlException ex)
            {

                throw new Exception($"Error BD al eliminar usuario: {ex.Message}", ex);

            }
        }

        public int Insertar(Usuario usuario)
        {
            try
            {
                using(var con = _conexion.ObtenerConexion())
                {

                    string sql = @"INSERT INTO usuarios (nombre, correo, telefono, contrasena, rol) VALUES (@Nombre, @Correo, @Telefono, @Contrasena, @Rol) RETURNING id";

                    using(var cmd = new NpgsqlCommand(sql, con))
                    {
                        AgregarParams(cmd, usuario);
                        con.Open();
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }

                }
            }
            catch (NpgsqlException ex)
            {

                throw new Exception($"Error BD al insertar usuario: {ex.Message}", ex);

            }
        }

        public Usuario ObtenerPorCorreo(string correo)
        {

            try
            {

                using(var con = _conexion.ObtenerConexion())
                {

                    string sql = $@"SELECT {Columnas} FROM usuarios WHERE correo = @Correo";
                    using (var cmd = new NpgsqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@Correo", correo);
                        con.Open();
                        using (var r = cmd.ExecuteReader())
                            if (r.Read()) return Mapear(r);
                    }

                }

            }
            catch(NpgsqlException ex)
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

                    string sql = $@"SELECT {Columnas} FROM usuarios WHERE id = @Id";
                    using (var cmd = new NpgsqlCommand(sql, con))
                    {

                        cmd.Parameters.AddWithValue(@"Id", id);
                        con.Open();
                        using (var r = cmd.ExecuteReader())
                            if (r.Read()) return Mapear(r);

                    }

                }

            }
            catch(NpgsqlException ex)
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

                using(var con = _conexion.ObtenerConexion())
                {

                    string sql = $@"SELECT {Columnas} FROM usuarios";
                    using (var cmd = new NpgsqlCommand(sql, con))
                    {
                        con.Open();
                        using (var r = cmd.ExecuteReader())
                            while (r.Read())
                            { lista.Add(Mapear(r));
                            }
                    }
                }

            }
            catch (NpgsqlException ex)
            {

                throw new Exception($"Error BD al listar usuarios: {ex.Message}");

            }
            return lista;
        }

        public void ActualizarPerfil(Usuario usuario)
        {
            try
            {
                using (var con = _conexion.ObtenerConexion())
                {
                    string sql = @"UPDATE usuarios
                           SET correo = @Correo, telefono = @Telefono, foto = @Foto
                           WHERE id = @Id";
                    using (var cmd = new NpgsqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@Id", usuario.Id);
                        cmd.Parameters.AddWithValue("@Correo", usuario.Correo);
                        cmd.Parameters.AddWithValue("@Telefono", usuario.Telefono);

                        var paramFoto = new NpgsqlParameter("@Foto", NpgsqlDbType.Bytea);
                        paramFoto.Value = (object)usuario.Foto ?? DBNull.Value;
                        cmd.Parameters.Add(paramFoto);

                        con.Open();
                        cmd.ExecuteNonQuery();
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
                    string sql = "UPDATE usuarios SET contrasena = @Contrasena WHERE id = @Id";
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

        public int ContarPublicaciones(int usuarioId)
        {
            string sql = "SELECT COUNT(*) FROM publicaciones WHERE vendedor_id = @Id AND activo = TRUE";
            return ContarPorUsuario(sql, usuarioId, "las publicaciones");
        }

        public int ContarCompras(int usuarioId)
        {
            string sql = "SELECT COUNT(*) FROM solicitudes_compra WHERE comprador_id = @Id AND estado = 'Aceptada'";
            return ContarPorUsuario(sql, usuarioId, "las compras");
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

        private Usuario Mapear(NpgsqlDataReader r) => new Usuario
        {

            Id = Convert.ToInt32(r["id"]),
            Nombre = r["nombre"].ToString(),
            Correo = r["Correo"].ToString(),
            Telefono = r["telefono"].ToString(),
            Contrasena = r["contrasena"].ToString(),
            Rol = (RolUsuario)Enum.Parse(typeof(RolUsuario), r["rol"].ToString()),
            Activo = Convert.ToBoolean(r["activo"]),
            FechaRegistro = Convert.ToDateTime(r["fecha_registro"]).ToLocalTime(),
            Foto = r.IsDBNull(r.GetOrdinal("foto")) ? null : (byte[])r["foto"]

        };

        private void AgregarParams(NpgsqlCommand cmd, Usuario usuario)
        {

            cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
            cmd.Parameters.AddWithValue("@Correo", usuario.Correo);
            cmd.Parameters.AddWithValue("@Telefono", usuario.Telefono);
            cmd.Parameters.AddWithValue("@Contrasena", usuario.Contrasena);
            cmd.Parameters.AddWithValue("@Rol", usuario.Rol.ToString());

        }

    }
}
