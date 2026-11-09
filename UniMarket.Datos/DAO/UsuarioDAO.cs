using Microsoft.Extensions.Logging.Abstractions;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniMarket.Entidades;

namespace UniMarket.Datos.DAO
{
    public class UsuarioDAO : IUsuarioDAO
    {

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
                        cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                        cmd.Parameters.AddWithValue("@Correo", usuario.Correo);
                        cmd.Parameters.AddWithValue("@Telefono", usuario.Telefono);
                        cmd.Parameters.AddWithValue("@Contrasena", usuario.Contrasena);
                        cmd.Parameters.AddWithValue("@Rol", usuario.Rol);
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

                    string sql = "UDPATE usuarios SET activo = FALSE WHERE id = @Id";
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
                        cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                        cmd.Parameters.AddWithValue("@Correo", usuario.Correo);
                        cmd.Parameters.AddWithValue("@Telefono", usuario.Telefono);
                        cmd.Parameters.AddWithValue("@Contrasena", usuario.Contrasena);
                        cmd.Parameters.AddWithValue("@Rol", usuario.Rol);
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

                    string sql = @"SELECT id, nombre, correo, telefono, contrasena, rol, activo FROM usuarios WHERE correo = @Correo";
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

                    string sql = @"SELECT id, nombre, correo, telefono, contrasena, rol, activo FROM usuarios WHERE id = @Id";
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

                    string sql = @"SELECT id, nombre, correo, telefono, contrasena, rol, activo FROM usuarios";

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

                throw new Exception($"Error BD al listar usuarios: {ex.Message}");

            }
            return lista;
        }

        private Usuario Mapear(NpgsqlDataReader r) => new Usuario
        {

            Id = Convert.ToInt32(r["id"]),
            Nombre = r["nombre"].ToString(),
            Correo = r["Correo"].ToString(),
            Telefono = r["telefono"].ToString(),
            Contrasena = r["contrasena"].ToString(),
            Rol = r["rol"].ToString(),
            Activo = Convert.ToBoolean(r["activo"])

        };
    }
}
