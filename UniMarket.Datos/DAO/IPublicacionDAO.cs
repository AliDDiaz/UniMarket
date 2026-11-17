using System.Collections.Generic;
using UniMarket.Entidades;

namespace UniMarket.Datos.DAO
{
    public interface IPublicacionDAO
    {
        List<Publicacion> ObtenerDisponibles();
        List<Publicacion> Buscar(string texto);
        List<Publicacion> ObtenerPorVendedor(int vendedorId);
        Publicacion ObtenerPorId(int id);
        int Insertar(Publicacion publicacion);
        void Actualizar(Publicacion publicacion);
        void Eliminar(int id);
    }
}