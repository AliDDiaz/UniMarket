using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using UniMarket.Entidades;
using UniMarket.Logica;

namespace UniMarket.Presentacion
{

    public enum AccionDetalle { Ninguna, Editar, Eliminar }

    public partial class FrmDetalleProducto : Form
    {
        private readonly Publicacion _pub;
        private readonly Usuario _usuario;
        private readonly SolicitudService _solicitudes = new SolicitudService();
        // Lo que el dueño eligió hacer desde el detalle (lo lee FrmPrincipal)
        public AccionDetalle Accion { get; private set; } = AccionDetalle.Ninguna;

        public FrmDetalleProducto(Publicacion pub, Usuario usuario)
        {
            InitializeComponent();
            _pub = pub;
            _usuario = usuario;

            lblTitulo.Text = pub.Titulo;
            lblPrecio.Text = pub.AceptaVenta ? $"${pub.Precio:N0}" : "Solo trueque";
            txtDescripcion.Text = string.IsNullOrWhiteSpace(pub.Descripcion)
                ? "El vendedor no agregó una descripción."
                : pub.Descripcion;
            lblCantidad.Text = $"Unidades disponibles: {pub.Stock}";

            if (pub.Imagen != null && pub.Imagen.Length > 0)
            {
                using (var ms = new MemoryStream(pub.Imagen))
                using (var imagen = Image.FromStream(ms))
                {
                    picProducto.Image = new Bitmap(imagen);
                }
            }

            ConfigurarTrueque();
            ConfigurarPropietario();
        }

        // El apartado de trueque solo aparece si el vendedor lo activó
        private void ConfigurarTrueque()
        {
            bool acepta = _pub.AceptaTrueque;

            lblTrueque.Visible = acepta;
            txtTrueque.Visible = acepta;
            btnProponerTrueque.Visible = acepta;

            if (!acepta) return;

            txtTrueque.Text = _pub.TruequeBusca;

            if (_pub.PerteneceA(_usuario.Id))
                DeshabilitarBoton("Tu publicación");
            else if (_pub.Stock <= 0)
                DeshabilitarBoton("Agotado");
        }

        private void DeshabilitarBoton(string texto)
        {
            btnProponerTrueque.Text = texto;
            btnProponerTrueque.BackColor = Color.FromArgb(180, 170, 210);
            btnProponerTrueque.Enabled = false;
        }

        private void btnProponerTrueque_Click(object sender, EventArgs e)
        {
            int cantidad = 1;
            string oferta = "";

            // Si hay un error de validación, el cuadro vuelve a abrirse con lo escrito
            while (true)
            {
                if (!PedirPropuesta(ref cantidad, ref oferta))
                    return;

                try
                {
                    _solicitudes.ProponerTrueque(_pub.Id, _usuario.Id, cantidad, oferta);

                    MessageBox.Show("Tu propuesta fue enviada. El vendedor recibirá una notificación.",
                        "Trueque", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    DialogResult = DialogResult.OK;
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "No se pudo enviar la propuesta",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        // Cuadro para escribir la propuesta. Devuelve false si el usuario cancela.
        private bool PedirPropuesta(ref int cantidad, ref string oferta)
        {
            int maximo = Math.Max(1, _pub.Stock);

            using (var dialogo = new Form())
            {
                dialogo.Text = "Proponer trueque";
                dialogo.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialogo.StartPosition = FormStartPosition.CenterParent;
                dialogo.MaximizeBox = false;
                dialogo.MinimizeBox = false;
                dialogo.ShowInTaskbar = false;
                dialogo.ClientSize = new Size(400, 300);
                dialogo.BackColor = Color.Lavender;
                dialogo.Font = new Font("Segoe UI", 9F);

                var lblBusca = new Label
                {
                    Text = "El vendedor busca: " + _pub.TruequeBusca,
                    Location = new Point(20, 14),
                    Size = new Size(360, 48),
                    ForeColor = Color.Indigo,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold)
                };

                var lblCantidad = new Label
                {
                    Text = "Unidades que quieres:",
                    Location = new Point(20, 72),
                    AutoSize = true,
                    ForeColor = Color.Indigo
                };

                var nudCantidad = new NumericUpDown
                {
                    Location = new Point(200, 68),
                    Size = new Size(70, 25),
                    Minimum = 1,
                    Maximum = maximo,
                    Value = Math.Min(Math.Max(cantidad, 1), maximo)
                };

                var lblOferta = new Label
                {
                    Text = "¿Qué ofreces a cambio? (mínimo 10 caracteres)",
                    Location = new Point(20, 104),
                    AutoSize = true,
                    ForeColor = Color.Indigo
                };

                var txtOferta = new TextBox
                {
                    Location = new Point(20, 128),
                    Size = new Size(360, 100),
                    Multiline = true,
                    MaxLength = 300,
                    ScrollBars = ScrollBars.Vertical,
                    Text = oferta
                };

                var btnEnviar = new Button
                {
                    Text = "Enviar propuesta",
                    DialogResult = DialogResult.OK,
                    Location = new Point(150, 246),
                    Size = new Size(130, 34),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(103, 58, 183),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };
                btnEnviar.FlatAppearance.BorderSize = 0;

                var btnCancelar = new Button
                {
                    Text = "Cancelar",
                    DialogResult = DialogResult.Cancel,
                    Location = new Point(290, 246),
                    Size = new Size(90, 34),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(237, 231, 251),
                    ForeColor = Color.FromArgb(109, 63, 214),
                    Cursor = Cursors.Hand
                };
                btnCancelar.FlatAppearance.BorderSize = 0;

                dialogo.Controls.AddRange(new Control[]
                    { lblBusca, lblCantidad, nudCantidad, lblOferta, txtOferta, btnEnviar, btnCancelar });
                dialogo.CancelButton = btnCancelar;

                if (dialogo.ShowDialog(this) != DialogResult.OK)
                    return false;

                cantidad = (int)nudCantidad.Value;
                oferta = txtOferta.Text;
                return true;
            }
        }

        // Si la publicación es del usuario, en lugar de proponer trueque ve Editar y Eliminar
        private void ConfigurarPropietario()
        {
            if (!_pub.PerteneceA(_usuario.Id)) return;

            btnProponerTrueque.Visible = false;

            // Garantiza espacio para la fila de botones
            if (ClientSize.Height < 470)
                ClientSize = new Size(ClientSize.Width, 470);

            int y = ClientSize.Height - 56;   // siempre a 56 px del borde de abajo

            // Los dos botones se reparten el ancho del recuadro de descripción
            int separacion = 10;
            int ancho = (txtDescripcion.Width - separacion) / 2;
            int x1 = txtDescripcion.Left;
            int x2 = x1 + ancho + separacion;

            var btnEditar = CrearBotonAccion("Editar", new Point(x1, y), new Size(ancho, 38),
                Color.FromArgb(103, 58, 183), Color.White);
            btnEditar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnEditar.Click += (s, e) =>
            {
                Accion = AccionDetalle.Editar;
                DialogResult = DialogResult.OK;
            };

            var btnEliminar = CrearBotonAccion("Eliminar", new Point(x2, y), new Size(ancho, 38),
                Color.FromArgb(254, 226, 226), Color.FromArgb(185, 28, 28));
            btnEliminar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnEliminar.Click += (s, e) =>
            {
                Accion = AccionDetalle.Eliminar;
                DialogResult = DialogResult.OK;
            };

            Controls.Add(btnEditar);
            Controls.Add(btnEliminar);
            btnEditar.BringToFront();
            btnEliminar.BringToFront();
        }

        private Button CrearBotonAccion(string texto, Point ubicacion, Size tamano, Color fondo, Color letra)
        {
            var boton = new Button
            {
                Text = texto,
                Location = ubicacion,
                Size = tamano,
                FlatStyle = FlatStyle.Flat,
                BackColor = fondo,
                ForeColor = letra,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            boton.FlatAppearance.BorderSize = 0;
            return boton;
        }

        private void lblTitulo_Click(object sender, EventArgs e)
        {

        }

        private void FrmDetalleProducto_Load(object sender, EventArgs e)
        {

        }
    }
}