using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniMarket.Entidades;

namespace UniMarket.Presentacion
{
    public partial class FrmDetalleProducto : Form
    {

        public FrmDetalleProducto(Publicacion pub)
        {
            InitializeComponent();

            lblTitulo.Text = pub.Titulo;
            lblPrecio.Text = $"${pub.Precio:N0}";
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
        }

        private void lblTitulo_Click(object sender, EventArgs e)
        {

        }
    }
}
