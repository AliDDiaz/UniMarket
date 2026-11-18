using System;
using System.Windows.Forms;
using UniMarket.Entidades;
using UniMarket.Logica;

namespace UniMarket.Presentacion
{
    public partial class FrmLogin : Form
    {
        private readonly UsuarioService _service;

        public Usuario UsuarioLogueado { get; private set; }

        public FrmLogin()
        {
            InitializeComponent();
            _service = new UsuarioService();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            try
            {
                UsuarioLogueado = _service.IniciarSesion(txtCorreo.Text, txtContrasena.Text);
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "No se pudo iniciar sesión",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtContrasena.Clear();
                txtContrasena.Focus();
            }
        }

        private void btnRegistrarse_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmRegistro())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    txtContrasena.Clear();
                    txtCorreo.Focus();
                }
            }
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblContrasena_Click(object sender, EventArgs e)
        {

        }
    }
}