using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniMarket.Logica;

namespace UniMarket.Presentacion
{
    public partial class FrmRegistro : Form
    {
        private readonly UsuarioService _service;

        public FrmRegistro()
        {
            InitializeComponent();
            _service = new UsuarioService();
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtContrasena.Text != txtConfirmar.Text)
                    throw new Exception("Las contraseñas no coinciden.");

                _service.Registrar(
                    txtNombre.Text,
                    txtCorreo.Text,
                    txtTelefono.Text,
                    txtContrasena.Text);

                MessageBox.Show("Registro exitoso. Ya puedes iniciar sesión.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "No se pudo registrar",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }


        private void FrmRegistro_Load(object sender, EventArgs e)
        {

        }


        private void flowLayoutPanel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtTelefono_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblConfirmar_Click(object sender, EventArgs e)
        {

        }
    }
}
