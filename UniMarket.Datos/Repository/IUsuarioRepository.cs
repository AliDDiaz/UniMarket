using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniMarket.Entidades;

namespace UniMarket.Datos.Repository
{
    public interface IUsuarioRepository
    {

        List<Usuario> GetAll();
        Usuario GetById(int id);
        Usuario GetByCorreo(string correo);
        int Add(Usuario usuario);
        void Update(Usuario usuario);
        void Delete(int id);
        void UpdateProfile(Usuario usuario);
        void UpdatePassword(int id, string contrasenaHash);
        int CountPublications(int usuarioId);
        int CountPurchases(int usuarioId);

    }
}
