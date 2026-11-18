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
            while (true)
            {
                Usuario usuario;
                using (var login = new FrmLogin())
                {
                    // Si cierra el Login con la 'X' o cancela, termina el programa por completo
                    if (login.ShowDialog() != DialogResult.OK)
                        break;

                    usuario = login.UsuarioLogueado;
                }

                // Abre FrmPrincipal2; si se cierra con la 'X', termina el programa.
                // Si se cierra pulsando el botón Salir (DialogResult.Retry), el bucle vuelve al Login.
                using (var principal = new FrmPrincipal2(usuario))
                {
                    if (principal.ShowDialog() != DialogResult.Retry)
                        break;
                }
            }

        }
    }
}
