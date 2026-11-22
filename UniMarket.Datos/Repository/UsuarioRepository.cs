using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniMarket.Datos.DAO;
using UniMarket.Entidades;

namespace UniMarket.Datos.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {

        private readonly IUsuarioDAO _dao;

        public UsuarioRepository(IUsuarioDAO dao)
        {
            _dao = dao;
        }

        public List<Usuario> GetAll() => _dao.ObtenerTodos();
        public Usuario GetById(int id) => _dao.ObtenerPorId(id);
        public Usuario GetByCorreo(string correo) => _dao.ObtenerPorCorreo(correo);
        public int Add(Usuario usuario) => _dao.Insertar(usuario);
        public void Update(Usuario usuario) => _dao.Actualizar(usuario);
        public void Delete(int id) => _dao.Eliminar(id);
        public void UpdateProfile(Usuario usuario) => _dao.ActualizarPerfil(usuario);
        public void UpdatePassword(int id, string contrasenaHash) => _dao.ActualizarContrasena(id, contrasenaHash);
        public int CountPublications(int usuarioId) => _dao.ContarPublicaciones(usuarioId);
        public int CountPurchases(int usuarioId) => _dao.ContarCompras(usuarioId);
        public void DeactivateAccount(int usuarioId) => _dao.DesactivarCuenta(usuarioId);
        
    }
}
