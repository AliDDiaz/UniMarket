using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniMarket.Entidades;
using UniMarket.Logica;

namespace UniMarket.Presentacion
{
    public partial class    FrmPrincipal2 : Form
    {
        private readonly PublicacionService _service;
        private readonly Usuario _usuario;
        private Form formularioActivo = null;
        private readonly BotService _botService = new BotService();
        private int _ultimoIdNotificado = 0;


        public FrmPrincipal2(Usuario usuario)
        {
            InitializeComponent();
            _service = new PublicacionService();
            _usuario = usuario;
            AbrirFormularioEnPanel(new FrmPrincipal(_usuario));
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new FrmPublicacion(_usuario.Id));
        }
        private void button2_MouseEnter(object sender, EventArgs e)
        {
            button2.BackColor = Color.FromArgb(124, 77, 255); // Morado vibrante más luminoso
            button2.ForeColor = Color.White;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            button2.BackColor = Color.FromArgb(26, 20, 44);   // Morado oscuro del menú
            button2.ForeColor = Color.White;
        }


        private void btnSalir_Click(object sender, EventArgs e)
        {

            var confirmar = MessageBox.Show("¿Deseas cerrar sesión?", "Cerrar sesión",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmar == DialogResult.Yes)
            {
                // Le avisa a Program.cs que debe volver a abrir el Login y cierra FrmPrincipal2
                this.DialogResult = DialogResult.Retry;
                this.Close();
            }

        }
        private void btnSalir_MouseEnter(object sender, EventArgs e)
        {
            button2.BackColor = Color.FromArgb(108, 63, 214);
            button2.ForeColor = Color.White;
        }

        private void btnSalir_MouseLeave(object sender, EventArgs e)
        {
            button2.BackColor = Color.FromArgb(26, 20, 44);
            button2.ForeColor = Color.White;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new FrmPrincipal(_usuario));
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

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_MouseEnter_1(object sender, EventArgs e)
        {
            button1.BackColor = Color.FromArgb(124, 77, 255); 
            button1.ForeColor = Color.White;

        }

        private void button1_MouseLeave_1(object sender, EventArgs e)
        {
            button1.BackColor = Color.Indigo;   
            button1.ForeColor = Color.White;
        }

        private void button2_MouseEnter_1(object sender, EventArgs e)
        {
            button2.BackColor = Color.FromArgb(124, 77, 255); 
            button2.ForeColor = Color.White;
        }

        private void button2_MouseLeave_1(object sender, EventArgs e)
        {
            button2.BackColor = Color.Indigo;   
            button2.ForeColor = Color.White;
        }

        private void btnHistorial_MouseEnter(object sender, EventArgs e)
        {
            btnHistorial.BackColor = Color.FromArgb(124, 77, 255);
            btnHistorial.ForeColor = Color.White;
        }

        private void btnHistorial_MouseLeave(object sender, EventArgs e)
        {
            btnHistorial.BackColor = Color.Indigo;
            btnHistorial.ForeColor = Color.White;
        }

        private void btnMycuenta_MouseEnter(object sender, EventArgs e)
        {
            btnMycuenta.BackColor = Color.FromArgb(124, 77, 255);
            btnMycuenta.ForeColor = Color.White;
        }

        private void btnMycuenta_MouseLeave(object sender, EventArgs e)
        {
            btnMycuenta.BackColor = Color.Indigo;
            btnMycuenta.ForeColor = Color.White;
        }

        private void btnSalir_MouseEnter_1(object sender, EventArgs e)
        {
            btnSalir.BackColor = Color.FromArgb(124, 77, 255);
            btnSalir.ForeColor = Color.White;
        }

        private void btnSalir_MouseLeave_1(object sender, EventArgs e)
        {
            btnSalir.BackColor = Color.Indigo;
            btnSalir.ForeColor = Color.White;
        }




        private void timerBot_Tick(object sender, EventArgs e)
        {
            RevisarNotificacionesBot();
        }

        private void RevisarNotificacionesBot()
        {
            try
            {
                var noLeidos = _botService.RevisarNuevosMensajes(_usuario.Id);

                if (noLeidos.Count > 0)
                {
                    lblBadgeBot.Text = noLeidos.Count.ToString();
                    lblBadgeBot.Visible = true;

                    var ultimo = noLeidos[noLeidos.Count - 1];
                    if (ultimo.Id > _ultimoIdNotificado)
                    {
                        _ultimoIdNotificado = ultimo.Id;
                        notifyIconBot.ShowBalloonTip(
                            4500,
                            $"Trueque Bot - {ultimo.Titulo}",
                            ultimo.Mensaje,
                            ToolTipIcon.Info
                        );
                    }
                }
                else
                {
                    lblBadgeBot.Visible = false;
                }
            }
            catch
            {
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnBot_Click_1(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new FrmBot(_usuario.Id));
            lblBadgeBot.Visible = false;
        }

        private void FrmPrincipal2_Load_1(object sender, EventArgs e)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddEllipse(0, 0, lblBadgeBot.Width, lblBadgeBot.Height);
            lblBadgeBot.Region = new Region(path);
            lblBadgeBot.BringToFront();

            RevisarNotificacionesBot();
            timerBot.Start();
        }
    }
}
