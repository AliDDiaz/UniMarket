using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using UniMarket.Entidades;
using UniMarket.Logica;

namespace UniMarket.Presentacion
{
    public partial class FrmMiCuenta : Form
    {
        private const int MaxFotoBytes = 2 * 1024 * 1024;

        private readonly Usuario _usuario;
        private readonly UsuarioService _service = new UsuarioService();

        private byte[] _fotoActual;     // la que está guardada en la base de datos
        private byte[] _fotoNueva;      // la que eligió pero aún no guarda
        private bool _fotoCambiada;

        public FrmMiCuenta()
        {
            InitializeComponent();
        }

        public FrmMiCuenta(Usuario usuario) : this()
        {
            _usuario = usuario;
        }

        private void FrmMiCuenta_Load(object sender, EventArgs e)
        {
            if (_usuario == null) return;

            var path = new GraphicsPath();
            path.AddEllipse(0, 0, picFoto.Width, picFoto.Height);
            picFoto.Region = new Region(path);

            CargarPerfil();
            ActivarEdicion(false);
        }

        // ---------- Carga de datos ----------

        private void CargarPerfil()
        {
            try
            {
                var perfil = _service.ObtenerPerfil(_usuario.Id);

                txtNombre.Text = perfil.Nombre;
                txtCorreo.Text = perfil.Correo;
                txtTelefono.Text = perfil.Telefono;

                lblNombrePerfil.Text = perfil.Nombre;
                lblCorreoPerfil.Text = perfil.Correo;

                lblMiembroValor.Text = perfil.FechaRegistro
                    .ToString("MMM yyyy", new CultureInfo("es-CO"));
                lblPublicacionesValor.Text = _service.ContarPublicaciones(_usuario.Id).ToString();
                lblComprasValor.Text = _service.ContarCompras(_usuario.Id).ToString();

                _fotoActual = perfil.Foto;
                _fotoNueva = null;
                _fotoCambiada = false;
                MostrarFoto(_fotoActual);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo cargar tu cuenta: " + ex.Message,
                    "Mi cuenta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void MostrarFoto(byte[] bytes)
        {
            Image nueva = null;

            if (bytes != null && bytes.Length > 0)
            {
                using (var ms = new MemoryStream(bytes))
                using (var img = Image.FromStream(ms))
                {
                    nueva = new Bitmap(img);
                }
            }

            var anterior = picFoto.Image;
            picFoto.Image = nueva;
            if (anterior != null) anterior.Dispose();
        }

        // ---------- Modo consulta / modo edición ----------

        private void ActivarEdicion(bool editar)
        {
            Color fondo = editar ? Color.White : Color.FromArgb(245, 243, 255);

            txtCorreo.ReadOnly = !editar;
            txtTelefono.ReadOnly = !editar;
            txtCorreo.BackColor = fondo;
            txtTelefono.BackColor = fondo;

            pnlContrasena.Visible = editar;
            btnCambiarFoto.Enabled = editar;
            btnGuardar.Enabled = editar;
            btnCancelar.Enabled = editar;
            btnEditar.Enabled = !editar;

            if (!editar)
                LimpiarContrasenas();
        }

        private void LimpiarContrasenas()
        {
            txtContrasenaActual.Clear();
            txtContrasenaNueva.Clear();
            txtContrasenaConfirmar.Clear();
        }

        // ---------- Botones ----------

        private void btnEditar_Click(object sender, EventArgs e)
        {
            ActivarEdicion(true);
            txtCorreo.Focus();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            CargarPerfil();          // descarta lo escrito y vuelve a los datos guardados
            ActivarEdicion(false);
        }

        private void btnCambiarFoto_Click(object sender, EventArgs e)
        {
            using (var dialogo = new OpenFileDialog())
            {
                dialogo.Title = "Selecciona tu foto";
                dialogo.Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.bmp";

                if (dialogo.ShowDialog() != DialogResult.OK) return;

                if (new FileInfo(dialogo.FileName).Length > MaxFotoBytes)
                {
                    MessageBox.Show("La foto no puede pesar más de 2 MB.",
                        "Mi cuenta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    byte[] bytes = File.ReadAllBytes(dialogo.FileName);
                    MostrarFoto(bytes);      // si no es una imagen válida, falla aquí
                    _fotoNueva = bytes;
                    _fotoCambiada = true;
                }
                catch
                {
                    MessageBox.Show("El archivo seleccionado no es una imagen válida.",
                        "Mi cuenta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string actual = txtContrasenaActual.Text;
            string nueva = txtContrasenaNueva.Text;
            string confirmar = txtContrasenaConfirmar.Text;

            bool cambiaClave = actual.Length > 0 || nueva.Length > 0 || confirmar.Length > 0;

            if (cambiaClave)
            {
                if (actual.Length == 0)
                {
                    Aviso("Escribe tu contraseña actual para poder cambiarla.");
                    return;
                }
                if (nueva != confirmar)
                {
                    Aviso("La nueva contraseña y su confirmación no coinciden.");
                    return;
                }
            }

            // 1) Datos del perfil
            try
            {
                byte[] foto = _fotoCambiada ? _fotoNueva : _fotoActual;
                _service.ActualizarPerfil(_usuario.Id, txtCorreo.Text, txtTelefono.Text, foto);
            }
            catch (Exception ex)
            {
                Aviso(ex.Message);
                return;
            }

            // 2) Contraseña (solo si la escribió)
            if (cambiaClave)
            {
                try
                {
                    _service.CambiarContrasena(_usuario.Id, actual, nueva);
                }
                catch (Exception ex)
                {
                    CargarPerfil();
                    SincronizarUsuarioEnSesion();
                    Aviso("Tus datos se guardaron, pero la contraseña no se cambió: " + ex.Message);
                    return;
                }
            }

            CargarPerfil();
            SincronizarUsuarioEnSesion();
            ActivarEdicion(false);

            MessageBox.Show("Tu cuenta se actualizó correctamente.",
                "Mi cuenta", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Mantiene al usuario en memoria (el de la sesión) igual que la base de datos
        private void SincronizarUsuarioEnSesion()
        {
            _usuario.Correo = txtCorreo.Text;
            _usuario.Telefono = txtTelefono.Text;
            _usuario.Foto = _fotoActual;
        }

        private void Aviso(string mensaje)
        {
            MessageBox.Show(mensaje, "Mi cuenta",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}