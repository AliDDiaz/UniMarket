using System;
using System.Drawing;
using System.IO;
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
        private byte[] _imagenSeleccionada = null;

        public FrmPublicacion(int usuarioId, Publicacion publicacion = null)
        {
            InitializeComponent();
            _service = new PublicacionService();
            _usuarioId = usuarioId;
            _publicacion = publicacion;

            cmbModalidad.Items.AddRange(new object[]
            {
                "Solo venta (dinero)",
                "Solo trueque (intercambio)",
                "Venta o trueque"
            });
            cmbModalidad.SelectedIndex = 0;

            if (_publicacion == null)
            {
                Text = "Nueva publicación";
            }
            else
            {
                Text = "Editar publicación";
                txtTitulo.Text = _publicacion.Titulo;
                txtDescripcion.Text = _publicacion.Descripcion;
                if (_publicacion.AceptaVenta)
                    txtPrecio.Text = _publicacion.Precio.ToString("0.##");
                txtStock.Text = _publicacion.Stock.ToString();
                txtTrueque.Text = _publicacion.TruequeBusca;
                cmbModalidad.SelectedIndex = IndiceDeModalidad(_publicacion.Modalidad);

                _imagenSeleccionada = _publicacion.Imagen;
                if (_imagenSeleccionada != null && _imagenSeleccionada.Length > 0)
                {
                    using (var ms = new System.IO.MemoryStream(_imagenSeleccionada))
                    {
                        pbFoto.Image = System.Drawing.Image.FromStream(ms);
                        pbFoto.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                }
            }
        }

        // ----- Modalidad -----

        private static int IndiceDeModalidad(ModalidadPublicacion modalidad)
        {
            switch (modalidad)
            {
                case ModalidadPublicacion.Trueque: return 1;
                case ModalidadPublicacion.Ambas: return 2;
                default: return 0;
            }
        }

        private ModalidadPublicacion ModalidadSeleccionada()
        {
            switch (cmbModalidad.SelectedIndex)
            {
                case 1: return ModalidadPublicacion.Trueque;
                case 2: return ModalidadPublicacion.Ambas;
                default: return ModalidadPublicacion.Venta;
            }
        }

        // Solo se pueden escribir los campos que la modalidad elegida usa
        private void cmbModalidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            var modalidad = ModalidadSeleccionada();

            txtPrecio.Enabled = modalidad != ModalidadPublicacion.Trueque;
            txtTrueque.Enabled = modalidad != ModalidadPublicacion.Venta;
        }

        // ----- Guardar -----

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                var modalidad = ModalidadSeleccionada();

                decimal precio = 0;
                if (modalidad != ModalidadPublicacion.Trueque &&
                    !decimal.TryParse(txtPrecio.Text, out precio))
                    throw new Exception("El precio debe ser un número válido.");

                if (!int.TryParse(txtStock.Text, out int stock))
                    throw new Exception("Las unidades deben ser un número entero.");

                var publicacion = new Publicacion
                {
                    Titulo = txtTitulo.Text,
                    Descripcion = txtDescripcion.Text,
                    Precio = precio,
                    Stock = stock,
                    Imagen = _imagenSeleccionada,
                    Modalidad = modalidad,
                    TruequeBusca = txtTrueque.Text
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void btnSubirFoto_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Selecciona la foto del producto";
                ofd.Filter = "Imágenes (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        byte[] imagenBytes = System.IO.File.ReadAllBytes(ofd.FileName);

                        using (MemoryStream ms = new MemoryStream(imagenBytes))
                        using (Image imagen = Image.FromStream(ms))
                        {
                            Image nuevaImagen = new Bitmap(imagen);

                            Image imagenAnterior = pbFoto.Image;

                            pbFoto.Image = nuevaImagen;
                            pbFoto.SizeMode = PictureBoxSizeMode.Zoom;

                            if (imagenAnterior != null)
                                imagenAnterior.Dispose();
                        }

                        _imagenSeleccionada = imagenBytes;
                    }
                    catch (OutOfMemoryException)
                    {
                        MessageBox.Show("No se pudo cargar la imagen. Verifica que sea válida y no demasiado grande.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al cargar la imagen: " + ex.Message);
                    }
                }
            }
        }
    }
}