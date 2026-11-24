using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using UniMarket.Entidades;
using UniMarket.Logica;

namespace UniMarket.Presentacion
{
    public partial class FrmSolicitudes : Form
    {
        private readonly SolicitudService _service;
        private readonly Usuario _usuario;
        private List<SolicitudDetalle> _recibidas = new List<SolicitudDetalle>();
        private List<SolicitudDetalle> _enviadas = new List<SolicitudDetalle>();

        public FrmSolicitudes()
        {
            InitializeComponent();
        }

        public FrmSolicitudes(Usuario usuario) : this()
        {
            _service = new SolicitudService();
            _usuario = usuario;

            flpRecibidas.BringToFront();
            flpEnviadas.BringToFront();

            flpRecibidas.SizeChanged += (s, e) => RenderizarRecibidas();
            flpEnviadas.SizeChanged += (s, e) => RenderizarEnviadas();

            CargarListas();
        }

        private void CargarListas()
        {
            try
            {
                _recibidas = _service.ObtenerRecibidas(_usuario.Id);
                _enviadas = _service.ObtenerEnviadas(_usuario.Id);

                RenderizarRecibidas();
                RenderizarEnviadas();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar solicitudes: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RenderizarRecibidas()
        {
            flpRecibidas.Controls.Clear();

            if (_recibidas == null || _recibidas.Count == 0)
            {
                flpRecibidas.Controls.Add(CrearMensajeVacio("Aún no has recibido solicitudes.", flpRecibidas));
                return;
            }

            foreach (var s in _recibidas)
            {
                int ancho = Math.Max(280, flpRecibidas.ClientSize.Width - 26);
                bool mostrarConfirmarTrueque = s.EsTrueque && s.EstaAceptada;
                int altoCard = (s.EstaPendiente || mostrarConfirmarTrueque) ? 132 : 98;

                Panel card = new Panel
                {
                    Width = ancho,
                    Height = altoCard,
                    BackColor = Color.White,
                    Margin = new Padding(0, 0, 0, 10)
                };

                Label lblTitulo = new Label
                {
                    Text = s.Titulo,
                    Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(40, 40, 55),
                    Location = new Point(14, 12),
                    Size = new Size(ancho - 125, 22),
                    AutoEllipsis = true
                };

                Label lblEstado = CrearBadgeEstado(s.Estado.ToString(), ancho - 105, 12);

                string infoPago = s.EsTrueque
                    ? $"Oferta: {s.OfertaTrueque}"
                    : $"Total: ${s.Total:N0}";

                Label lblDetalle = new Label
                {
                    Text = $"Solicitante: {s.NombreContraparte}  •  {s.Cantidad} ud(s)  •  {infoPago}",
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(103, 58, 183),
                    Location = new Point(14, 40),
                    Size = new Size(ancho - 28, 20),
                    AutoEllipsis = true
                };

                Label lblFecha = new Label
                {
                    Text = $"📅 {s.Fecha:dd/MM/yyyy HH:mm}",
                    Font = new Font("Segoe UI", 8.5F),
                    ForeColor = Color.FromArgb(120, 120, 140),
                    Location = new Point(14, 64),
                    AutoSize = true
                };

                card.Controls.Add(lblTitulo);
                card.Controls.Add(lblEstado);
                card.Controls.Add(lblDetalle);
                card.Controls.Add(lblFecha);

                if (s.EstaPendiente)
                {
                    Button btnAceptarCard = new Button
                    {
                        Text = "✓ Aceptar",
                        Size = new Size(105, 30),
                        Location = new Point(14, 90),
                        BackColor = Color.FromArgb(34, 197, 94),
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                        Cursor = Cursors.Hand
                    };
                    btnAceptarCard.FlatAppearance.BorderSize = 0;
                    btnAceptarCard.Click += (sender, e) => AceptarSolicitud(s);

                    Button btnRechazarCard = new Button
                    {
                        Text = "✕ Rechazar",
                        Size = new Size(105, 30),
                        Location = new Point(127, 90),
                        BackColor = Color.FromArgb(239, 68, 68),
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                        Cursor = Cursors.Hand
                    };
                    btnRechazarCard.FlatAppearance.BorderSize = 0;
                    btnRechazarCard.Click += (sender, e) => RechazarSolicitud(s);

                    card.Controls.Add(btnAceptarCard);
                    card.Controls.Add(btnRechazarCard);
                }
                else if (mostrarConfirmarTrueque)
                {
                    Button btnConfirmar = CrearBotonConfirmarTrueque(s, esRecibida: true, x: 14, y: 90);
                    card.Controls.Add(btnConfirmar);
                }

                flpRecibidas.Controls.Add(card);
            }
        }

        private void RenderizarEnviadas()
        {
            flpEnviadas.Controls.Clear();

            if (_enviadas == null || _enviadas.Count == 0)
            {
                flpEnviadas.Controls.Add(CrearMensajeVacio("No has enviado solicitudes todavía.", flpEnviadas));
                return;
            }

            foreach (var s in _enviadas)
            {
                int ancho = Math.Max(280, flpEnviadas.ClientSize.Width - 26);
                bool tieneWhatsapp = !string.IsNullOrWhiteSpace(s.TelefonoContacto);
                bool mostrarConfirmarTrueque = s.EsTrueque && s.EstaAceptada;
                bool cabeEnUnaFila = ancho >= 445;

                int altoCard = (tieneWhatsapp && mostrarConfirmarTrueque && !cabeEnUnaFila) ? 168 : 132;

                Panel card = new Panel
                {
                    Width = ancho,
                    Height = altoCard,
                    BackColor = Color.White,
                    Margin = new Padding(0, 0, 0, 10)
                };

                Label lblTitulo = new Label
                {
                    Text = s.Titulo,
                    Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(40, 40, 55),
                    Location = new Point(14, 12),
                    Size = new Size(ancho - 125, 22),
                    AutoEllipsis = true
                };

                Label lblEstado = CrearBadgeEstado(s.Estado.ToString(), ancho - 105, 12);

                string vendedorTexto = string.IsNullOrWhiteSpace(s.NombreContraparte)
                    ? "Vendedor: (visible al aceptar)"
                    : $"Vendedor: {s.NombreContraparte}";

                string infoPago = s.EsTrueque
                    ? $"Oferta: {s.OfertaTrueque}"
                    : $"${s.Total:N0}";

                Label lblDetalle = new Label
                {
                    Text = $"{vendedorTexto}  •  {s.Cantidad} ud(s)  •  {infoPago}",
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(103, 58, 183),
                    Location = new Point(14, 40),
                    Size = new Size(ancho - 28, 20),
                    AutoEllipsis = true
                };

                Label lblFecha = new Label
                {
                    Text = $"📅 {s.Fecha:dd/MM/yyyy HH:mm}",
                    Font = new Font("Segoe UI", 8.5F),
                    ForeColor = Color.FromArgb(120, 120, 140),
                    Location = new Point(14, 64),
                    AutoSize = true
                };

                card.Controls.Add(lblTitulo);
                card.Controls.Add(lblEstado);
                card.Controls.Add(lblDetalle);
                card.Controls.Add(lblFecha);

                if (tieneWhatsapp)
                {
                    Button btnWhatsApp = new Button
                    {
                        Text = $"💬 WhatsApp: {s.TelefonoContacto}",
                        Size = new Size(215, 30),
                        Location = new Point(14, 90),
                        BackColor = Color.FromArgb(37, 211, 102),
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                        Cursor = Cursors.Hand
                    };
                    btnWhatsApp.FlatAppearance.BorderSize = 0;
                    btnWhatsApp.Click += (sender, e) =>
                        AbrirWhatsApp(s.TelefonoContacto, s.Titulo, s.Cantidad);

                    card.Controls.Add(btnWhatsApp);

                    if (mostrarConfirmarTrueque)
                    {
                        int xConfirmar = cabeEnUnaFila ? 237 : 14;
                        int yConfirmar = cabeEnUnaFila ? 90 : 126;
                        Button btnConfirmar = CrearBotonConfirmarTrueque(s, esRecibida: false, x: xConfirmar, y: yConfirmar);
                        card.Controls.Add(btnConfirmar);
                    }
                }
                else
                {
                    Label lblInfo = new Label
                    {
                        Text = s.EstaPendiente
                            ? "⏳ El contacto de WhatsApp aparecerá cuando acepten."
                            : "Contacto no disponible.",
                        Font = new Font("Segoe UI", 8.5F, FontStyle.Italic),
                        ForeColor = Color.FromArgb(130, 120, 155),
                        Location = new Point(14, 94),
                        AutoSize = true
                    };
                    card.Controls.Add(lblInfo);
                }

                flpEnviadas.Controls.Add(card);
            }
        }

        private Button CrearBotonConfirmarTrueque(SolicitudDetalle s, bool esRecibida, int x, int y)
        {
            bool yaConfirmeYo = esRecibida ? s.ConfirmadoDueno : s.ConfirmadoSolicitante;
            bool completado = s.TruequeCompletado;

            string textoBoton = completado
                ? "✔ Trueque completado"
                : (yaConfirmeYo ? "⏳ Esperando contraparte" : "🤝 Confirmar trueque");

            Color colorFondo = completado
                ? Color.FromArgb(21, 128, 61)
                : (yaConfirmeYo ? Color.FromArgb(148, 163, 184) : Color.FromArgb(103, 58, 183));

            Button btn = new Button
            {
                Text = textoBoton,
                Enabled = !yaConfirmeYo && !completado,
                Size = new Size(195, 30),
                Location = new Point(x, y),
                BackColor = colorFondo,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.8F, FontStyle.Bold),
                Cursor = (!yaConfirmeYo && !completado) ? Cursors.Hand : Cursors.Default
            };
            btn.FlatAppearance.BorderSize = 0;

            if (!yaConfirmeYo && !completado)
            {
                btn.Click += (sender, e) => ConfirmarTruequeSolicitud(s);
            }

            return btn;
        }

        private void ConfirmarTruequeSolicitud(SolicitudDetalle s)
        {
            var conf = MessageBox.Show(
                $"¿Confirmas que ya realizaste el intercambio físico por \"{s.Titulo}\"?\n\n" +
                "Cuando ambos estudiantes confirmen, el trueque quedará marcado como Completado.",
                "Confirmar trueque", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (conf != DialogResult.Yes) return;

            try
            {
                if (_service.ConfirmarTrueque(s.Id, _usuario.Id))
                {
                    MessageBox.Show("Tu confirmación del trueque ha sido registrada.", "Trueque",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No se pudo registrar la confirmación.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al confirmar trueque",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            CargarListas();
        }

        private Label CrearBadgeEstado(string estado, int x, int y)
        {
            string est = (estado ?? "").ToUpper();
            Color fondo = Color.FromArgb(254, 243, 199);
            Color texto = Color.FromArgb(180, 83, 9);

            if (est.Contains("ACEPT"))
            {
                fondo = Color.FromArgb(220, 252, 231);
                texto = Color.FromArgb(21, 128, 61);
            }
            else if (est.Contains("RECHAZ"))
            {
                fondo = Color.FromArgb(254, 226, 226);
                texto = Color.FromArgb(185, 28, 28);
            }

            return new Label
            {
                Text = est,
                Location = new Point(x, y),
                Size = new Size(92, 22),
                BackColor = fondo,
                ForeColor = texto,
                Font = new Font("Segoe UI", 7.8F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };
        }

        private Label CrearMensajeVacio(string mensaje, FlowLayoutPanel contenedor)
        {
            return new Label
            {
                Text = mensaje,
                AutoSize = false,
                Width = Math.Max(250, contenedor.ClientSize.Width - 26),
                Height = 50,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(110, 100, 135),
                Padding = new Padding(4, 10, 4, 4)
            };
        }

        private void AceptarSolicitud(SolicitudDetalle s)
        {
            var conf = MessageBox.Show(
                $"¿Aceptar la solicitud de {s.NombreContraparte} por {s.Cantidad} unidad(es) de \"{s.Titulo}\"?\n\n" +
                "Se descontarán las unidades de tu publicación y el solicitante verá tus datos de contacto.",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (conf != DialogResult.Yes) return;

            try
            {
                _service.Aceptar(s.Id, _usuario.Id);
                MessageBox.Show("Solicitud aceptada.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "No se pudo aceptar",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            CargarListas();
        }

        private void RechazarSolicitud(SolicitudDetalle s)
        {
            var conf = MessageBox.Show(
                $"¿Rechazar la solicitud de {s.NombreContraparte} por \"{s.Titulo}\"?",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (conf != DialogResult.Yes) return;

            try
            {
                _service.Rechazar(s.Id, _usuario.Id);
                MessageBox.Show("Solicitud rechazada.", "Listo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "No se pudo rechazar",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            CargarListas();
        }

        private void AbrirWhatsApp(string telefono, string producto, int cantidad)
        {
            string soloNumeros = new string(telefono.Where(char.IsDigit).ToArray());

            if (soloNumeros.Length == 10 && soloNumeros.StartsWith("3"))
            {
                soloNumeros = "57" + soloNumeros;
            }

            string mensaje = Uri.EscapeDataString(
                $"¡Hola! Te contacto desde Trueque. Me aceptaste la solicitud por {cantidad} unidad(es) de \"{producto}\".");

            string url = $"https://wa.me/{soloNumeros}?text={mensaje}";

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo abrir WhatsApp: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarListas();
        }
    }
}