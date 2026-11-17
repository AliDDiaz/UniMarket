using System;
using System.Windows.Forms;
using UniMarket.Entidades;
using UniMarket.Logica;

namespace UniMarket.Presentacion
{
    public partial class FrmSolicitudes : Form
    {
        private readonly SolicitudService _service;
        private readonly Usuario _usuario;

        public FrmSolicitudes(Usuario usuario)
        {
            InitializeComponent();
            _service = new SolicitudService();
            _usuario = usuario;

            PrepararGrilla(dgvRecibidas);
            AgregarColumna(dgvRecibidas, "Producto", "Titulo", 35);
            AgregarColumna(dgvRecibidas, "Comprador", "NombreContraparte", 25);
            AgregarColumna(dgvRecibidas, "Cantidad", "Cantidad", 12);
            AgregarColumna(dgvRecibidas, "Total", "Total", 18, "#,##0.##");
            AgregarColumna(dgvRecibidas, "Estado", "Estado", 15);
            AgregarColumna(dgvRecibidas, "Fecha", "Fecha", 20, "dd/MM/yyyy HH:mm");

            PrepararGrilla(dgvEnviadas);
            AgregarColumna(dgvEnviadas, "Producto", "Titulo", 30);
            AgregarColumna(dgvEnviadas, "Cantidad", "Cantidad", 10);
            AgregarColumna(dgvEnviadas, "Total", "Total", 15, "#,##0.##");
            AgregarColumna(dgvEnviadas, "Estado", "Estado", 12);
            AgregarColumna(dgvEnviadas, "Vendedor", "NombreContraparte", 20, null, "(al aceptar)");
            AgregarColumna(dgvEnviadas, "Teléfono", "TelefonoContacto", 15, null, "(al aceptar)");
            AgregarColumna(dgvEnviadas, "Fecha", "Fecha", 18, "dd/MM/yyyy HH:mm");

            CargarListas();
        }

        private void PrepararGrilla(DataGridView dgv)
        {
            dgv.AutoGenerateColumns = false;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.MultiSelect = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void AgregarColumna(DataGridView dgv, string titulo, string propiedad,
                                    float peso, string formato = null, string textoNulo = null)
        {
            var estilo = new DataGridViewCellStyle();
            if (formato != null) estilo.Format = formato;
            if (textoNulo != null) estilo.NullValue = textoNulo;

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = titulo,
                DataPropertyName = propiedad,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = peso,
                DefaultCellStyle = estilo
            });
        }

        private void CargarListas()
        {
            try
            {
                dgvRecibidas.DataSource = null;
                dgvRecibidas.DataSource = _service.ObtenerRecibidas(_usuario.Id);

                dgvEnviadas.DataSource = null;
                dgvEnviadas.DataSource = _service.ObtenerEnviadas(_usuario.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private SolicitudDetalle ObtenerPendienteSeleccionada()
        {
            var s = dgvRecibidas.CurrentRow?.DataBoundItem as SolicitudDetalle;

            if (s == null)
            {
                MessageBox.Show("Selecciona una solicitud de la lista.", "Atención",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }

            if (!s.EstaPendiente)
            {
                MessageBox.Show("Esta solicitud ya fue atendida.", "Atención",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }

            return s;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            var s = ObtenerPendienteSeleccionada();
            if (s == null) return;

            var conf = MessageBox.Show(
                $"¿Aceptar la solicitud de {s.NombreContraparte} por {s.Cantidad} unidad(es) de \"{s.Titulo}\"?\n\n" +
                "Se descontarán las unidades de tu publicación y el comprador verá tus datos de contacto.",
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

        private void btnRechazar_Click(object sender, EventArgs e)
        {
            var s = ObtenerPendienteSeleccionada();
            if (s == null) return;

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

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarListas();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}