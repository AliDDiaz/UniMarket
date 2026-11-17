using System;
using System.Windows.Forms;
using UniMarket.Entidades;
using UniMarket.Logica;

namespace UniMarket.Presentacion
{
    public partial class FrmPublicacion : Form
    {
        private readonly PublicacionService _service;
        private readonly int _usuarioId;
        private readonly Publicacion _publicacion;

        public FrmPublicacion(int usuarioId, Publicacion publicacion = null)
        {
            InitializeComponent();
            _service = new PublicacionService();
            _usuarioId = usuarioId;
            _publicacion = publicacion;

            if (_publicacion == null)
            {
                Text = "Nueva publicación";
            }
            else
            {
                Text = "Editar publicación";
                txtTitulo.Text = _publicacion.Titulo;
                txtDescripcion.Text = _publicacion.Descripcion;
                txtPrecio.Text = _publicacion.Precio.ToString("0.##");
                txtStock.Text = _publicacion.Stock.ToString();
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!decimal.TryParse(txtPrecio.Text, out decimal precio))
                    throw new Exception("El precio debe ser un número válido.");
                if (!int.TryParse(txtStock.Text, out int stock))
                    throw new Exception("Las unidades deben ser un número entero.");

                var publicacion = new Publicacion
                {
                    Titulo = txtTitulo.Text,
                    Descripcion = txtDescripcion.Text,
                    Precio = precio,
                    Stock = stock
                };

                if (_publicacion == null)
                {
                    publicacion.VendedorId = _usuarioId;
                    _service.Publicar(publicacion);
                    MessageBox.Show("Publicación creada.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    publicacion.Id = _publicacion.Id;
                    _service.Editar(publicacion, _usuarioId);
                    MessageBox.Show("Publicación actualizada.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "No se pudo guardar",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}