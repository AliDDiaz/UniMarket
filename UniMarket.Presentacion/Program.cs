using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniMarket.Entidades;

namespace UniMarket.Presentacion
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Usuario usuario;
            using (var login = new FrmLogin())
            {
                if (login.ShowDialog() != DialogResult.OK) return;
                usuario = login.UsuarioLogueado;
            }

            Application.Run(new FrmPrincipal2(usuario));
        }
    }
}
