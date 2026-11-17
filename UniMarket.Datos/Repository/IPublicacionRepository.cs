using System.Collections.Generic;
using UniMarket.Entidades;

namespace UniMarket.Datos.Repository
{
    public interface IPublicacionRepository
    {
        List<Publicacion> GetAvailable();
        List<Publicacion> GetByVendedor(int vendedorId);
        List<Publicacion> Search(string texto);
        Publicacion GetById(int id);
        int Add(Publicacion publicacion);
        void Update(Publicacion publicacion);
        void Delete(int id);
    }
}