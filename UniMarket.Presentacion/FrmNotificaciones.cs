using System;
using System.Drawing;
using System.Windows.Forms;
using UniMarket.Entidades;
using UniMarket.Logica;

namespace UniMarket.Presentacion
{
    public partial class FrmNotificaciones : Form
    {
        private readonly int _usuarioId;
        private readonly NotificacionesService _botService = new NotificacionesService();

        public FrmNotificaciones()
        {
            InitializeComponent();
        }

        public FrmNotificaciones(int usuarioId) : this()
        {
            _usuarioId = usuarioId;
        }

        private void FrmBot_Load(object sender, EventArgs e)
        {
            CargarHistorial();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarHistorial();
        }

        private void flpMensajes_SizeChanged(object sender, EventArgs e)
        {
            int nuevoAncho = Math.Max(280, flpMensajes.ClientSize.Width - 55);
            foreach (Control card in flpMensajes.Controls)
            {
                card.Width = nuevoAncho;
                foreach (Control hijo in card.Controls)
                {
                    if (hijo is Label lbl && !lbl.AutoSize)
                        lbl.Width = nuevoAncho - 30;
                }
            }
        }

        private void CargarHistorial()
        {
            flpMensajes.Controls.Clear();

            try
            {
                var mensajes = _botService.ObtenerHistorial(_usuarioId);

                if (mensajes.Count == 0)
                {
                    flpMensajes.Controls.Add(new Label
                    {
                        Text = "¡Hola! Aquí te avisaremos cuando recibas ofertas o respondan tus solicitudes.",
                        AutoSize = false,
                        Width = Math.Max(280, flpMensajes.ClientSize.Width - 55),
                        Height = 70,
                        ForeColor = Color.FromArgb(90, 80, 120),
                        Font = new Font("Segoe UI", 10.5F),
                        Padding = new Padding(10)
                    });
                    return;
                }

                foreach (var msg in mensajes)
                    flpMensajes.Controls.Add(CrearBurbujaMensaje(msg));

                flpMensajes_SizeChanged(this, EventArgs.Empty);
                _botService.MarcarComoLeidos(_usuarioId);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudieron cargar las notificaciones: " + ex.Message,
                    "Notificaciones", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private Panel CrearBurbujaMensaje(MensajeNotificaciones msg)
        {
            Color colorBorde = Color.FromArgb(124, 77, 255);
            if (msg.Tipo == "OFERTA_ACEPTADA") colorBorde = Color.FromArgb(46, 204, 113);
            if (msg.Tipo == "OFERTA_RECHAZADA") colorBorde = Color.FromArgb(231, 76, 60);

            int ancho = Math.Max(280, flpMensajes.ClientSize.Width - 55);

            var card = new Panel
            {
                Width = ancho,
                Height = 95,
                BackColor = msg.Leido ? Color.White : Color.FromArgb(237, 231, 255),
                Margin = new Padding(0, 0, 0, 10)
            };

            card.Controls.Add(new Label
            {
                Text = msg.Mensaje,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(60, 60, 75),
                Location = new Point(16, 36),
                AutoSize = false,
                Width = ancho - 30,
                Height = 52
            });

            card.Controls.Add(new Label
            {
                Text = $" {msg.Titulo}   •   {msg.Fecha:dd/MM/yyyy HH:mm}",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(46, 26, 94),
                Location = new Point(16, 10),
                AutoSize = true
            });

            card.Controls.Add(new Panel
            {
                Width = 6,
                Dock = DockStyle.Left,
                BackColor = colorBorde
            });

            return card;
        }
    }
}