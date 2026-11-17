using System.Collections.Generic;
using UniMarket.Datos.DAO;
using UniMarket.Entidades;

namespace UniMarket.Datos.Repository
{
    public class PublicacionRepository : IPublicacionRepository
    {
        private readonly IPublicacionDAO _dao;

        public PublicacionRepository(IPublicacionDAO dao)
        {
            _dao = dao;
        }

        public List<Publicacion> GetAvailable() => _dao.ObtenerDisponibles();
        public List<Publicacion> GetByVendedor(int vendedorId) => _dao.ObtenerPorVendedor(vendedorId);
        public List<Publicacion> Search(string texto) => _dao.Buscar(texto);
        public Publicacion GetById(int id) => _dao.ObtenerPorId(id);
        public int Add(Publicacion p) => _dao.Insertar(p);
        public void Update(Publicacion p) => _dao.Actualizar(p);
        public void Delete(int id) => _dao.Eliminar(id);
    }
}