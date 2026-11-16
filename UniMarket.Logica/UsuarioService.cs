using System;
using System.Net.Mail;
using System.Text.RegularExpressions;
using UniMarket.Datos.DAO;
using UniMarket.Datos.Repository;
using UniMarket.Entidades;

namespace UniMarket.Logica
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _repository;

        public UsuarioService(IUsuarioRepository repository)
            => _repository = repository;

        // Constructor de conveniencia
        public UsuarioService()
            : this(new UsuarioRepository(new UsuarioDAO())) { }

        public int Registrar(string nombre, string correo, string telefono, string contrasena)
        {
            nombre = (nombre ?? "").Trim();
            correo = (correo ?? "").Trim().ToLowerInvariant();
            telefono = (telefono ?? "").Trim();

            ValidarDatos(nombre, correo, telefono);
            ValidarContrasena(contrasena);

            if (_repository.GetByCorreo(correo) != null)
                throw new Exception("Ya existe un usuario con ese correo.");

            var usuario = new Usuario
            {
                Nombre = nombre,
                Correo = correo,
                Telefono = telefono,
                Contrasena = HashContrasena.Generar(contrasena),
                Rol = "Estudiante"
            };

            return _repository.Add(usuario);
        }

        public Usuario IniciarSesion(string correo, string contrasena)
        {
            correo = (correo ?? "").Trim().ToLowerInvariant();

            if (correo == "" || string.IsNullOrEmpty(contrasena))
                throw new Exception("Ingresa tu correo y tu contraseña.");

            var usuario = _repository.GetByCorreo(correo);

            if (usuario == null || !HashContrasena.Verificar(contrasena, usuario.Contrasena))
                throw new Exception("Correo o contraseña incorrectos.");

            if (!usuario.Activo)
                throw new Exception("Esta cuenta está desactivada.");

            usuario.Contrasena = null;
            return usuario;
        }

        private void ValidarDatos(string nombre, string correo, string telefono)
        {
            if (nombre == "")
                throw new Exception("El nombre es obligatorio.");

            if (!EsCorreoValido(correo))
                throw new Exception("El correo no es válido.");

            if (!Regex.IsMatch(telefono, @"^\d{7,15}$"))
                throw new Exception("El teléfono debe tener solo números (entre 7 y 15 dígitos).");
        }

        private void ValidarContrasena(string contrasena)
        {
            if (string.IsNullOrEmpty(contrasena) || contrasena.Length < 6)
                throw new Exception("La contraseña debe tener al menos 6 caracteres.");
        }

        private bool EsCorreoValido(string correo)
        {
            try
            {
                return new MailAddress(correo).Address == correo;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}