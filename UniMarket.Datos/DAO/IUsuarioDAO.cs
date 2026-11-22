using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniMarket.Entidades;

namespace UniMarket.Datos.DAO
{
    public interface IUsuarioDAO
    {

        List<Usuario> ObtenerTodos();
        Usuario ObtenerPorId(int id);
        Usuario ObtenerPorCorreo(string correo);
        int Insertar(Usuario usuario);
        void Actualizar(Usuario usuario);
        void Eliminar(int id);
        void ActualizarPerfil(Usuario usuario);
        void ActualizarContrasena(int id, string contrasenaHash);
        int ContarPublicaciones(int usuarioId);
        int ContarCompras(int usuarioId);
        void DesactivarCuenta(int usuarioId);
    }
}
