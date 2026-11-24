using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using UniMarket.Entidades;
using UniMarket.Logica;

namespace UniMarket.Presentacion
{
    public partial class FrmPrincipal : Form
    {
        private readonly PublicacionService _service;
        private Publicacion _publicacionSeleccionada;
        private readonly Usuario _usuario;
        private BotAyuda botAyuda = new BotAyuda();
        public FrmPrincipal(Usuario usuario)
        {
            InitializeComponent();
            this.Controls.Add(botAyuda);
            _service = new PublicacionService();
            _usuario = usuario;
            flpCatalogo.SizeChanged += (s, e) => AjustarTarjetas();
            CargarPublicaciones();
        }

        private void CargarPublicaciones()
        {
            try
            {
                _publicacionSeleccionada = null;

                var lista = chkMisPublicaciones.Checked
                    ? _service.ObtenerMisPublicaciones(_usuario.Id)
                    : _service.ObtenerCatalogo(txtBuscar.Text);

                MostrarCatalogo(lista);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Publicacion ObtenerSeleccionada()
        {
            if (_publicacionSeleccionada == null)
            {
                MessageBox.Show("Haz clic sobre una tarjeta de producto para seleccionarla.", "Atención",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            return _publicacionSeleccionada;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarPublicaciones();
        }

        private void chkMisPublicaciones_CheckedChanged(object sender, EventArgs e)
        {
            txtBuscar.Enabled = !chkMisPublicaciones.Checked;
            btnBuscar.Enabled = !chkMisPublicaciones.Checked;
            CargarPublicaciones();
        }

        private void EditarPublicacion(Publicacion publicacion)
        {
            // El catálogo vive dentro de FrmPrincipal2: se le pide abrir la edición en su panel,
            // así el menú de la izquierda sigue funcionando
            var principal = Parent?.FindForm() as FrmPrincipal2;
            if (principal != null)
            {
                principal.AbrirEdicion(publicacion);
                return;
            }

            // Respaldo si el catálogo se abre solo (sin la ventana principal)
            using (var frm = new FrmPublicacion(_usuario.Id, publicacion))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                    CargarPublicaciones();
            }
        }

        private void EliminarPublicacion(Publicacion publicacion)
        {
            var conf = MessageBox.Show($"¿Eliminar \"{publicacion.Titulo}\"?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (conf != DialogResult.Yes) return;

            try
            {
                _service.Eliminar(publicacion.Id, _usuario.Id);
                CargarPublicaciones();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "No se pudo eliminar",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void MostrarCatalogo(List<Publicacion> lista)
        {
            flpCatalogo.Controls.Clear();

            foreach (var pub in lista)
            {
                // 1. Tarjeta blanca
                Panel card = new Panel
                {
                    Width = CalcularAnchoTarjeta(),
                    Height = 275,
                    BackColor = Color.White,
                    Margin = new Padding(12),
                    Cursor = Cursors.Hand
                };

                card.Click += (s, e) => VerDetalleProducto(card, pub);

                // 2. Foto del producto arriba
                PictureBox pic = new PictureBox
                {
                    Size = new Size(196, 130),
                    Location = new Point(12, 12),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BackColor = Color.FromArgb(248, 247, 252)
                };
                if (pub.Imagen != null && pub.Imagen.Length > 0)
                {
                    using (var ms = new MemoryStream(pub.Imagen))
                        pic.Image = Image.FromStream(ms);
                }
                pic.Click += (s, e) => VerDetalleProducto(card, pub);

                // 3. Título del producto
                Label lblTitulo = new Label
                {
                    Text = pub.Titulo,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(40, 40, 55),
                    Location = new Point(12, 150),
                    Size = new Size(196, 22),
                    AutoEllipsis = true
                };
                lblTitulo.Click += (s, e) => VerDetalleProducto(card, pub);

                // 4. Precio grande en morado (o "Solo trueque" si no tiene precio)
                Label lblPrecio = new Label
                {
                    Text = pub.AceptaVenta ? $"${pub.Precio:N0}" : "Solo trueque",
                    Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(103, 58, 183),
                    Location = new Point(12, 174),
                    AutoSize = true
                };
                lblPrecio.Click += (s, e) => VerDetalleProducto(card, pub);

                // Aviso cuando acepta dinero o trueque
                Label lblAceptaTrueque = null;
                if (pub.AceptaVenta && pub.AceptaTrueque)
                {
                    lblAceptaTrueque = new Label
                    {
                        Text = "⇄ También acepta trueque",
                        Font = new Font("Segoe UI", 8.5F),
                        ForeColor = Color.FromArgb(109, 63, 214),
                        Location = new Point(12, 200),
                        AutoSize = true
                    };
                    lblAceptaTrueque.Click += (s, e) => VerDetalleProducto(card, pub);
                }

                // 5. Botón morado "Agregar al carrito"
                Button btnAgregar = new Button
                {
                    Text = "🛒 Agregar al carrito",
                    Size = new Size(196, 36),
                    Location = new Point(12, 222),
                    BackColor = Color.FromArgb(103, 58, 183),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };
                btnAgregar.FlatAppearance.BorderSize = 0;

                if (pub.PerteneceA(_usuario.Id))
                {
                    btnAgregar.Text = "Tu publicación";
                    btnAgregar.BackColor = Color.FromArgb(180, 170, 210);
                    btnAgregar.Enabled = false;
                }
                else if (!pub.AceptaVenta)
                {
                    btnAgregar.Text = "⇄ Ver y proponer trueque";
                    btnAgregar.Click += (s, e) => VerDetalleProducto(card, pub);
                }
                else
                {
                    btnAgregar.Click += (s, e) =>
                    {
                        try
                        {
                            CarritoService.Agregar(pub, 1);

                            // Feedback visual rápido en el mismo botón sin interrumpir con MessageBox
                            btnAgregar.Text = "✓ Agregado";
                            btnAgregar.BackColor = Color.FromArgb(46, 204, 113);

                            var t = new Timer { Interval = 1200 };
                            t.Tick += (ts, te) =>
                            {
                                btnAgregar.Text = "🛒 Agregar al carrito";
                                btnAgregar.BackColor = Color.FromArgb(103, 58, 183);
                                t.Stop();
                                t.Dispose();
                            };
                            t.Start();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Stock disponible",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    };
                }

                card.Resize += (s, e) =>
                {
                    int interior = card.Width - 24;
                    pic.Width = interior;
                    lblTitulo.Width = interior;
                    btnAgregar.Width = interior;
                };
                card.Width = card.Width;   // fuerza un primer ajuste
                card.Controls.Add(pic);
                card.Controls.Add(lblTitulo);
                card.Controls.Add(lblPrecio);
                card.Controls.Add(btnAgregar);
                if (lblAceptaTrueque != null)
                card.Controls.Add(lblAceptaTrueque);


                flpCatalogo.Controls.Add(card);
            }

            AjustarTarjetas();

        }

        private void SeleccionarTarjeta(Panel tarjetaElegida, Publicacion pub)
        {
            _publicacionSeleccionada = pub;
            foreach (Control ctrl in flpCatalogo.Controls)
                ctrl.BackColor = Color.White;

            tarjetaElegida.BackColor = Color.FromArgb(237, 231, 255);
        }
        private void VerDetalleProducto(Panel tarjeta, Publicacion pub)
        {
            SeleccionarTarjeta(tarjeta, pub);

            AccionDetalle accion;
            using (var detalle = new FrmDetalleProducto(pub, _usuario))
            {
                detalle.ShowDialog(this);
                accion = detalle.Accion;
            }

            // Se ejecuta después de cerrar el detalle, para no abrir un cuadro encima de otro
            if (accion == AccionDetalle.Editar) EditarPublicacion(pub);
            else if (accion == AccionDetalle.Eliminar) EliminarPublicacion(pub);
        }

        // Calcula cuántas columnas caben y reparte el ancho entre ellas
        private int CalcularAnchoTarjeta()
        {
            const int margen = 24;          // 12 a cada lado de la tarjeta
            const int anchoMinimo = 230;

            // Si todavía no hay barra de desplazamiento, se reserva su espacio para que no empuje las tarjetas
            int barra = flpCatalogo.VerticalScroll.Visible ? 0 : SystemInformation.VerticalScrollBarWidth;
            int disponible = flpCatalogo.ClientSize.Width - flpCatalogo.Padding.Horizontal - barra;
            if (disponible < anchoMinimo + margen) return anchoMinimo;

            int columnas = disponible / (anchoMinimo + margen);
            return disponible / columnas - margen;
        }

        private void AjustarTarjetas()
        {
            int ancho = CalcularAnchoTarjeta();

            flpCatalogo.SuspendLayout();
            foreach (Control tarjeta in flpCatalogo.Controls)
                if (tarjeta.Width != ancho)
                    tarjeta.Width = ancho;
            flpCatalogo.ResumeLayout();
        }

        private void picEngel_Click_1(object sender, EventArgs e)
        {
            botAyuda.Alternar();
        }


    }
}