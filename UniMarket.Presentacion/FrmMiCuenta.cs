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
                lblTruequesValor.Text = _service.ContarTrueques(_usuario.Id).ToString();

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

        private void btnEliminarCuenta_Click(object sender, EventArgs e)
        {
            var confirmar = MessageBox.Show(
                "Tu cuenta y tus publicaciones se desactivarán, y tus solicitudes pendientes " +
                "serán rechazadas.\n\nNo podrás volver a iniciar sesión con esta cuenta.\n\n" +
                "¿Deseas continuar?",
                "Desactivar cuenta",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (confirmar != DialogResult.Yes) return;

            string contrasena = PedirContrasena();
            if (contrasena == null) return;   // canceló

            try
            {
                _service.DesactivarCuenta(_usuario.Id, contrasena);
            }
            catch (Exception ex)
            {
                Aviso(ex.Message);
                return;
            }

            MessageBox.Show("Tu cuenta fue desactivada. Se cerrará tu sesión.",
                "Desactivar cuenta", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Cierra FrmPrincipal2 y le avisa a Program.cs que vuelva al Login
            var principal = Parent != null ? Parent.FindForm() : null;
            if (principal != null)
            {
                principal.DialogResult = DialogResult.Retry;
                principal.Close();
            }
        }

        // Cuadro pequeño para escribir la contraseña. Devuelve null si cancela.
        private string PedirContrasena()
        {
            using (var dialogo = new Form())
            {
                dialogo.Text = "Confirmar contraseña";
                dialogo.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialogo.StartPosition = FormStartPosition.CenterParent;
                dialogo.MaximizeBox = false;
                dialogo.MinimizeBox = false;
                dialogo.ShowInTaskbar = false;
                dialogo.ClientSize = new Size(340, 140);
                dialogo.BackColor = Color.FromArgb(245, 243, 255);
                dialogo.Font = new Font("Segoe UI", 9F);

                var lbl = new Label
                {
                    Text = "Escribe tu contraseña para confirmar:",
                    Location = new Point(20, 16),
                    AutoSize = true,
                    ForeColor = Color.FromArgb(59, 31, 154)
                };

                var txt = new TextBox
                {
                    Location = new Point(20, 44),
                    Size = new Size(300, 27),
                    Font = new Font("Segoe UI", 10F),
                    UseSystemPasswordChar = true
                };

                var btnAceptar = new Button
                {
                    Text = "Desactivar",
                    DialogResult = DialogResult.OK,
                    Location = new Point(110, 92),
                    Size = new Size(100, 34),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(220, 38, 38),
                    ForeColor = Color.White,
                    Cursor = Cursors.Hand
                };
                btnAceptar.FlatAppearance.BorderSize = 0;

                var btnCancelarDlg = new Button
                {
                    Text = "Cancelar",
                    DialogResult = DialogResult.Cancel,
                    Location = new Point(220, 92),
                    Size = new Size(100, 34),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(237, 231, 251),
                    ForeColor = Color.FromArgb(109, 63, 214),
                    Cursor = Cursors.Hand
                };
                btnCancelarDlg.FlatAppearance.BorderSize = 0;

                dialogo.Controls.AddRange(new Control[] { lbl, txt, btnAceptar, btnCancelarDlg });
                dialogo.AcceptButton = btnAceptar;
                dialogo.CancelButton = btnCancelarDlg;

                var resultado = dialogo.ShowDialog(Parent != null ? Parent.FindForm() : this);

                return resultado == DialogResult.OK ? txt.Text : null;
            }
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }
    }
}