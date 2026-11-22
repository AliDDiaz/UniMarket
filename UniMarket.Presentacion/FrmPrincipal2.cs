using System;
using System.Drawing;
using System.Windows.Forms;
using UniMarket.Entidades;
using UniMarket.Logica;

namespace UniMarket.Presentacion
{
    public partial class FrmPrincipal2 : Form
    {
        // Colores del menú lateral (un solo lugar para cambiarlos)
        private static readonly Color ColorNormal = Color.Indigo;
        private static readonly Color ColorHover = Color.FromArgb(124, 77, 255);

        private readonly Usuario _usuario;
        private readonly NotificacionesService _botService = new NotificacionesService();
        private Form formularioActivo = null;
        private int _ultimoIdNotificado = 0;

        public FrmPrincipal2(Usuario usuario)
        {
            InitializeComponent();
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

        private void FrmPrincipal2_Load_1(object sender, EventArgs e)
        {
            // Contador de notificaciones con forma circular
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddEllipse(0, 0, lblBadgeBot.Width, lblBadgeBot.Height);
            lblBadgeBot.Region = new Region(path);
            lblBadgeBot.BringToFront();

            RevisarNotificacionesBot();
            timerBot.Start();

            CarritoService.OnCarritoActualizado += ActualizarBotonCarrito;
            ActualizarBotonCarrito();
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
        }

        // ---------- Notificaciones ----------

        private void btnBot_Click_1(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new FrmNotificaciones(_usuario.Id));
            lblBadgeBot.Visible = false;
        }

        private void timerBot_Tick(object sender, EventArgs e)
        {
            RevisarNotificacionesBot();
        }

        private void RevisarNotificacionesBot()
        {
            try
            {
                var noLeidos = _botService.RevisarNuevosMensajes(_usuario.Id);

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
            catch
            {
                // Se ignoran los errores de red para no interrumpir al usuario
            }
        }
    }
}