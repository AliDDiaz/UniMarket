using System;
using System.Windows.Forms;
using UniMarket.Entidades;
using UniMarket.Logica;

namespace UniMarket.Presentacion
{
    public partial class FrmSolicitud : Form
    {
        private readonly SolicitudService _service;
        private readonly Publicacion _publicacion;
        private readonly int _compradorId;

        public FrmSolicitud(Publicacion publicacion, int compradorId)
        {
            InitializeComponent();
            _service = new SolicitudService();
            _publicacion = publicacion;
            _compradorId = compradorId;

            lblTitulo.Text = publicacion.Titulo;
            lblPrecio.Text = $"Precio por unidad: {publicacion.Precio:#,##0.##}";
            lblDisponibles.Text = $"Unidades disponibles: {publicacion.Stock}";

            nudCantidad.Minimum = 1;
            nudCantidad.Maximum = Math.Max(1, publicacion.Stock);
            nudCantidad.Value = 1;
        }

        private void FrmSolicitud_Load(object sender, EventArgs e)
        {

        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                _service.Solicitar(_publicacion.Id, _compradorId, (int)nudCantidad.Value);

                MessageBox.Show(
                    "Solicitud enviada. Cuando el vendedor la acepte, verás sus datos de contacto en \"Mis solicitudes\".",
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "No se pudo enviar la solicitud",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}