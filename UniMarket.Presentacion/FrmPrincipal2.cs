using System;
using System.Drawing;
using System.Windows.Forms;
using UniMarket.Entidades;

namespace UniMarket.Presentacion
{
    public partial class FrmPrincipal2 : Form
    {
        // Colores del menú lateral (un solo lugar para cambiarlos)
        private static readonly Color ColorNormal = Color.Indigo;
        private static readonly Color ColorHover = Color.FromArgb(124, 77, 255);

        private readonly Usuario _usuario;
        private Form formularioActivo = null;

        public FrmPrincipal2(Usuario usuario)
        {
            InitializeComponent();
            _usuario = usuario;

            ConfigurarHover(btnInicio, btnPublicar, btnHistorial, btnMycuenta, btnSalir);
            AbrirFormularioEnPanel(new FrmPrincipal(_usuario));
        }

        // Aplica el efecto hover a todos los botones que reciba
        private void ConfigurarHover(params Button[] botones)
        {
            foreach (var boton in botones)
            {
                boton.BackColor = ColorNormal;
                boton.ForeColor = Color.White;
                boton.MouseEnter += (s, e) => boton.BackColor = ColorHover;
                boton.MouseLeave += (s, e) => boton.BackColor = ColorNormal;
            }
        }

        private void AbrirFormularioEnPanel(Form formularioHijo)
        {
            if (formularioActivo != null)
                formularioActivo.Close();

            formularioActivo = formularioHijo;
            formularioHijo.TopLevel = false;
            formularioHijo.FormBorderStyle = FormBorderStyle.None;
            formularioHijo.Dock = DockStyle.Fill;

            panelContenedor.Controls.Clear();
            panelContenedor.Controls.Add(formularioHijo);
            panelContenedor.Tag = formularioHijo;
            formularioHijo.BringToFront();
            formularioHijo.Show();
        }

        // Inicio
        private void button1_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new FrmPrincipal(_usuario));
        }

        // Publicar
        private void button2_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new FrmPublicacion(_usuario.Id));
        }

        // Mi cuenta (pendiente: aún no hace nada, pero el Designer ya lo tiene enlazado)
        private void btnMycuenta_Click(object sender, EventArgs e)
        {

        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            var confirmar = MessageBox.Show("¿Deseas cerrar sesión?", "Cerrar sesión",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmar == DialogResult.Yes)
            {
                // Le avisa a Program.cs que debe volver a abrir el Login
                DialogResult = DialogResult.Retry;
                Close();
            }
        }
    }
}