using System;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using UniMarket.Datos.DAO;
using UniMarket.Datos.Repository;
using UniMarket.Entidades;

namespace UniMarket.Logica
{
    public class UsuarioService
    {
        private const int MaxFotoBytes = 2 * 1024 * 1024; // 2 MB
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
                Rol = RolUsuario.Estudiante
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

        public Usuario ObtenerPerfil(int usuarioId)
        {
            var usuario = _repository.GetById(usuarioId);

            if (usuario == null || !usuario.Activo)
                throw new Exception("No se encontró tu cuenta.");

            usuario.Contrasena = null;
            return usuario;
        }

        public void ActualizarPerfil(int usuarioId, string correo, string telefono, byte[] foto)
        {
            correo = (correo ?? "").Trim().ToLowerInvariant();
            telefono = (telefono ?? "").Trim();

            var usuario = _repository.GetById(usuarioId);
            if (usuario == null || !usuario.Activo)
                throw new Exception("No se encontró tu cuenta.");

            ValidarDatos(usuario.Nombre, correo, telefono);

            var otro = _repository.GetByCorreo(correo);
            if (otro != null && otro.Id != usuarioId)
                throw new Exception("Ya existe un usuario con ese correo.");

            if (foto != null && foto.Length > MaxFotoBytes)
                throw new Exception("La foto no puede pesar más de 2 MB.");

            usuario.Correo = correo;
            usuario.Telefono = telefono;
            usuario.Foto = foto;

            _repository.UpdateProfile(usuario);
        }

        public void CambiarContrasena(int usuarioId, string actual, string nueva)
        {
            var usuario = _repository.GetById(usuarioId);
            if (usuario == null || !usuario.Activo)
                throw new Exception("No se encontró tu cuenta.");

            if (!HashContrasena.Verificar(actual ?? "", usuario.Contrasena))
                throw new Exception("La contraseña actual no es correcta.");

            ValidarContrasena(nueva);

            if (HashContrasena.Verificar(nueva, usuario.Contrasena))
                throw new Exception("La nueva contraseña debe ser distinta a la actual.");

            _repository.UpdatePassword(usuarioId, HashContrasena.Generar(nueva));
        }

        public int ContarPublicaciones(int usuarioId) => _repository.CountPublications(usuarioId);

        public int ContarCompras(int usuarioId) => _repository.CountPurchases(usuarioId);
        public int ContarTrueques(int usuarioId) => _repository.CountBarters(usuarioId);

        private void ValidarDatos(string nombre, string correo, string telefono)
        {
            if (string.IsNullOrWhiteSpace(nombre) || !nombre.All(c => char.IsLetter(c) || c == ' '))
                throw new Exception("El nombre es obligatorio y solo puede contener letras y espacios.");

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

        public void DesactivarCuenta(int usuarioId, string contrasena)
        {
            if (string.IsNullOrWhiteSpace(contrasena))
                throw new Exception("Escribe tu contraseña para confirmar.");

            var usuario = _repository.GetById(usuarioId);

            if (usuario == null || !usuario.Activo)
                throw new Exception("No se encontró tu cuenta.");

            if (!HashContrasena.Verificar(contrasena, usuario.Contrasena))
                throw new Exception("La contraseña es incorrecta.");

            _repository.DeactivateAccount(usuarioId);
        }
    }
}