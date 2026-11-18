using System;
using System.Windows.Forms;
using UniMarket.Entidades;
using UniMarket.Logica;

namespace UniMarket.Presentacion
{
    public partial class FrmPrincipal : Form
    {
        private readonly PublicacionService _service;
        private readonly Usuario _usuario;

        public FrmPrincipal(Usuario usuario)
        {
            InitializeComponent();
            _service = new PublicacionService();
            _usuario = usuario;

            lblUsuario.Text = $"Hola, {_usuario.Nombre}";
            ConfigurarGrilla();
            CargarPublicaciones();
        }

        private void ConfigurarGrilla()
        {
            dgvPublicaciones.AutoGenerateColumns = false;

            dgvPublicaciones.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Título",
                DataPropertyName = "Titulo",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 40
            });
            dgvPublicaciones.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Descripción",
                DataPropertyName = "Descripcion",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 60
            });
            dgvPublicaciones.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Precio",
                DataPropertyName = "Precio",
                Width = 110,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "#,##0.##" }
            });
            dgvPublicaciones.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Unidades",
                DataPropertyName = "Stock",
                Width = 80
            });
        }

        private void CargarPublicaciones()
        {
            try
            {
                dgvPublicaciones.DataSource = null;

                dgvPublicaciones.DataSource = chkMisPublicaciones.Checked
                    ? _service.ObtenerMisPublicaciones(_usuario.Id)
                    : _service.ObtenerCatalogo(txtBuscar.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Publicacion ObtenerSeleccionada()
        {
            var publicacion = dgvPublicaciones.CurrentRow?.DataBoundItem as Publicacion;

            if (publicacion == null)
                MessageBox.Show("Selecciona una publicación de la lista.", "Atención",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

            return publicacion;
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

        private void btnEditar_Click(object sender, EventArgs e)
        {
            var publicacion = ObtenerSeleccionada();
            if (publicacion == null) return;

            if (publicacion.VendedorId != _usuario.Id)
            {
                MessageBox.Show("Solo puedes editar tus propias publicaciones.", "Atención",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var frm = new FrmPublicacion(_usuario.Id, publicacion))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                    CargarPublicaciones();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            var publicacion = ObtenerSeleccionada();
            if (publicacion == null) return;

            if (publicacion.VendedorId != _usuario.Id)
            {
                MessageBox.Show("Solo puedes eliminar tus propias publicaciones.", "Atención",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

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

        private void btnSolicitar_Click(object sender, EventArgs e)
        {
            var p = ObtenerSeleccionada();
            if (p == null) return;

            if (p.PerteneceA(_usuario.Id))
            {
                MessageBox.Show("No puedes comprar tu propia publicación.", "Atención",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var frm = new FrmSolicitud(p, _usuario.Id))
            {
                frm.ShowDialog();
            }
        }

        private void btnSolicitudes_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmSolicitudes(_usuario))
            {
                frm.ShowDialog();
            }

            CargarPublicaciones();
        }

        private void dgvPublicaciones_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}