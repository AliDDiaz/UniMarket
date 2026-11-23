using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniMarket.Entidades;
using UniMarket.Logica;

namespace UniMarket.Presentacion
{
    public partial class FrmPrincipal2 : Form
    {
        //---color 
        private static readonly Color ColorNormal = Color.Indigo;
        private static readonly Color ColorHover = Color.FromArgb(124, 77, 255);
        //-----color
        private readonly Usuario _usuario;
        private readonly NotificacionesService _botService = new NotificacionesService();
        private Form formularioActivo = null;
        private int _ultimoIdNotificado = 0;
        private bool _revisandoNotificaciones = false;

        public FrmPrincipal2(Usuario usuario)
        {
            InitializeComponent();
   
            FormClosed += FrmPrincipal2_FormClosed;
            _usuario = usuario;


            ConfigurarHover(button1, button2, btnHistorial, btnMycuenta, btnSalir);
            AbrirFormularioEnPanel(new FrmPrincipal(_usuario));
        }

        // Aplica el efecto hover del menú a todos los botones que reciba
        private void ConfigurarHover(params Button[] botones)
        {
            foreach (var boton in botones)
            {
                boton.MouseEnter += (s, e) =>
                {
                    boton.BackColor = ColorHover;
                    boton.ForeColor = Color.White;
                };
                boton.MouseLeave += (s, e) =>
                {
                    boton.BackColor = ColorNormal;
                    boton.ForeColor = Color.White;
                };
            }
        }

        // Carga un formulario hijo dentro del panel central
        private void AbrirFormularioEnPanel(Form formularioHijo)
        {
            if (formularioActivo != null)
                formularioActivo.Close();

            formularioActivo = formularioHijo;
            formularioHijo.TopLevel = false;
            formularioHijo.FormBorderStyle = FormBorderStyle.None;
            formularioHijo.Dock = DockStyle.Fill;

            panelContenedor.Controls.Clear();
            panelContenedor.Controls.Add(formularioHijo);
            panelContenedor.Tag = formularioHijo;
            formularioHijo.BringToFront();
            formularioHijo.Show();
        }

        private async void FrmPrincipal2_Load_1(object sender, EventArgs e)
        {
            // Contadores (campana y carrito) con forma circular
            HacerCircular(lblBadgeBot);
            HacerCircular(lblBadgeCarrito);

            timerBot.Start();
            await RevisarNotificacionesBotAsync();

            CarritoService.OnCarritoActualizado += ActualizarBotonCarrito;
            ActualizarBotonCarrito();
        }

        private void HacerCircular(Label etiqueta)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddEllipse(0, 0, etiqueta.Width, etiqueta.Height);
            etiqueta.Region = new Region(path);
            etiqueta.BringToFront();
        }

        private void FrmPrincipal2_FormClosed(object sender, FormClosedEventArgs e)
        {
            timerBot.Stop();
            // El carrito es estático: se limpia para que no pase al siguiente usuario
            CarritoService.OnCarritoActualizado -= ActualizarBotonCarrito;
            CarritoService.Vaciar();
        }

        // ---------- Menú lateral ----------

        // Inicio
        private void button1_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new FrmPrincipal(_usuario));
        }

        // Publicar
        private void button2_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new FrmPublicacion(_usuario.Id));
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            var confirmar = MessageBox.Show("¿Deseas cerrar sesión?", "Cerrar sesión",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmar == DialogResult.Yes)
            {
                // Le avisa a Program.cs que debe volver a abrir el Login
                DialogResult = DialogResult.Retry;
                Close();
            }
        }

        // ---------- Carrito ----------

        private void btnCarrito_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new FrmCarrito(_usuario));
        }

        private void ActualizarBotonCarrito()
        {
            int total = CarritoService.TotalUnidades;
            lblBadgeCarrito.Text = total.ToString();
            lblBadgeCarrito.Visible = total > 0;
        }

        // ---------- Notificaciones ----------

        private void btnBot_Click_1(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new FrmNotificaciones(_usuario.Id));
            lblBadgeBot.Visible = false;
        }

        private async void timerBot_Tick(object sender, EventArgs e)
        {
            await RevisarNotificacionesBotAsync();
        }

        private async Task RevisarNotificacionesBotAsync()
        {
            if (_revisandoNotificaciones) return;   // ya hay una consulta en curso
            _revisandoNotificaciones = true;

            try
            {
                int usuarioId = _usuario.Id;

                // La consulta a la base de datos corre en otro hilo
                var noLeidos = await Task.Run(() => _botService.RevisarNuevosMensajes(usuarioId));

                // De aquí en adelante estamos de vuelta en el hilo de la interfaz
                if (IsDisposed) return;   // el usuario pudo cerrar la ventana mientras esperaba

                if (noLeidos.Count > 0)
                {
                    lblBadgeBot.Text = noLeidos.Count.ToString();
                    lblBadgeBot.Visible = true;

                    var ultimo = noLeidos[noLeidos.Count - 1];
                    if (ultimo.Id > _ultimoIdNotificado)
                    {
                        _ultimoIdNotificado = ultimo.Id;
                        notifyIconBot.ShowBalloonTip(
                            4500,
                            $"Trueque Bot - {ultimo.Titulo}",
                            ultimo.Mensaje,
                            ToolTipIcon.Info
                        );
                    }
                }
                else
                {
                    lblBadgeBot.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Notificaciones] No se pudo revisar: {ex.Message}");
            }
            finally
            {
                _revisandoNotificaciones = false;
            }
        }
        private void btnMycuenta_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new FrmMiCuenta(_usuario));
        }

        private void btnHistorial_Click(object sender, EventArgs e)
        {

            AbrirFormularioEnPanel(new FrmSolicitudes(_usuario));
        }


    }
}

