using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using UniMarket.Entidades;
using UniMarket.Logica;
using System.Linq;

namespace UniMarket.Presentacion
{
    public partial class FrmCarrito : Form
    {
        private readonly int _compradorId;
        private readonly SolicitudService _solicitudService = new SolicitudService();

        private readonly Usuario _usuario;

        public FrmCarrito(Usuario usuario) : this()
        {
            _usuario = usuario;
            _compradorId = usuario.Id;
            flpItemsCarrito.BringToFront();
            RenderizarCarrito();
        }
        public FrmCarrito()
        {
            InitializeComponent();
        }


        private void FrmCarrito_Load(object sender, EventArgs e)
        {
            flpItemsCarrito.BringToFront();
            flpItemsCarrito.SizeChanged += (s, ev) => RenderizarCarrito();
            RenderizarCarrito();
        }

        private void RenderizarCarrito()
        {
            flpItemsCarrito.Controls.Clear();
            var items = CarritoService.ObtenerItems();

            if (items.Count == 0)
            {
                flpItemsCarrito.Controls.Add(new Label
                {
                    Text = "🛒 Tu carrito está vacío. Agrega productos desde el catálogo.",
                    AutoSize = false,
                    Width = Math.Max(300, flpItemsCarrito.ClientSize.Width - 35),
                    Height = 60,
                    Font = new Font("Segoe UI", 10.5F),
                    ForeColor = Color.FromArgb(90, 80, 120),
                    Padding = new Padding(10)
                });
                lblTotal.Text = "Total: $0";
                btnConfirmarCompra.Enabled = false;
                return;
            }

            btnConfirmarCompra.Enabled = true;

            foreach (var item in items)
            {
                Panel fila = new Panel
                {
                    Width = Math.Max(420, flpItemsCarrito.ClientSize.Width - 40), // <--- AQUÍ VA ESA LÍNEA
                    Height = 75,
                    BackColor = Color.White,
                    Margin = new Padding(0, 0, 0, 8)
                };

                PictureBox pic = new PictureBox
                {
                    Size = new Size(55, 55),
                    Location = new Point(10, 10),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BackColor = Color.FromArgb(245, 243, 255)
                };
                if (item.Producto.Imagen != null && item.Producto.Imagen.Length > 0)
                {
                    using (var ms = new MemoryStream(item.Producto.Imagen))
                        pic.Image = Image.FromStream(ms);
                }

                Label lblNombre = new Label
                {
                    Text = item.Producto.Titulo,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    Location = new Point(75, 12),
                    Size = new Size(190, 22),
                    AutoEllipsis = true
                };

                Label lblSub = new Label
                {
                    Text = $"${item.Producto.Precio:N0} c/u  •  Subtotal: ${item.Subtotal:N0}",
                    Font = new Font("Segoe UI", 9F),
                    ForeColor = Color.FromArgb(103, 58, 183),
                    Location = new Point(75, 38),
                    AutoSize = true
                };

                Button btnMenos = new Button
                {
                    Text = "-",
                    Size = new Size(28, 28),
                    Location = new Point(fila.Width - 135, 23),
                    FlatStyle = FlatStyle.Flat
                };
                btnMenos.Click += (s, e) =>
                {
                    CarritoService.CambiarCantidad(item.Producto.Id, item.Cantidad - 1);
                    RenderizarCarrito();
                };

                Label lblCant = new Label
                {
                    Text = item.Cantidad.ToString(),
                    Size = new Size(30, 28),
                    Location = new Point(fila.Width - 105, 23),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
                };

                Button btnMas = new Button
                {
                    Text = "+",
                    Size = new Size(28, 28),
                    Location = new Point(fila.Width - 73, 23),
                    FlatStyle = FlatStyle.Flat
                };
                btnMas.Click += (s, e) =>
                {
                    try
                    {
                        CarritoService.CambiarCantidad(item.Producto.Id, item.Cantidad + 1);
                        RenderizarCarrito();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                };

                Button btnQuitar = new Button
                {
                    Text = "✕",
                    Size = new Size(28, 28),
                    Location = new Point(fila.Width - 38, 23),
                    BackColor = Color.FromArgb(239, 68, 68),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                btnQuitar.FlatAppearance.BorderSize = 0;
                btnQuitar.Click += (s, e) =>
                {
                    CarritoService.Eliminar(item.Producto.Id);
                    RenderizarCarrito();
                };

                fila.Controls.AddRange(new Control[] { pic, lblNombre, lblSub, btnMenos, lblCant, btnMas, btnQuitar });
                flpItemsCarrito.Controls.Add(fila);
            }

            lblTotal.Text = $"Total ({CarritoService.TotalUnidades} uds): ${CarritoService.TotalPagar:N0}";
        }

        private void btnConfirmarCompra_Click_1(object sender, EventArgs e)
        {
            // Copia de los productos, porque el carrito cambia mientras se procesa
            var resultado = _solicitudService.EnviarCarrito(
                _compradorId, CarritoService.ObtenerItems().ToList());

            // Los productos enviados salen del carrito; los que fallaron se quedan
            foreach (var publicacionId in resultado.Enviados)
                CarritoService.Eliminar(publicacionId);

            RenderizarCarrito();

            if (resultado.Fallidos.Count == 0)
            {
                MessageBox.Show("¡Solicitudes enviadas! Los vendedores han sido notificados por Trueque Bot.",
                    "Compra solicitada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string encabezado = resultado.Enviados.Count > 0
                ? $"Se enviaron {resultado.Enviados.Count} solicitud(es), pero estas no se pudieron enviar y siguen en tu carrito:"
                : "No se pudo enviar ninguna solicitud:";

            MessageBox.Show(
                encabezado + Environment.NewLine + Environment.NewLine +
                string.Join(Environment.NewLine, resultado.Fallidos),
                "Compra solicitada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btnVaciar_Click_1(object sender, EventArgs e)
        {
            CarritoService.Vaciar();
            RenderizarCarrito();
        }
    }
}