using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniMarket.Presentacion
{
    public partial class FrmBienvenida : Form
    {
        public FrmBienvenida()
        {
            InitializeComponent();
            btnComenzar.Parent = picBienvenida;
            btnComenzar.BackColor = Color.Transparent;
            btnCerrar.Parent = picBienvenida;
            btnCerrar.BackColor = Color.Transparent;
            this.Opacity = 0;
            timerBienvenida.Start();
        }

        private void btnComenzar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void timerBienvenida_Tick(object sender, EventArgs e)
        {
            this.Opacity += 0.05;

            if (this.Opacity >= 1)
            {
                this.Opacity = 1;
                timerBienvenida.Stop();
            }
        }
    }
}
