using System;
using System.Collections.Generic;
using UniMarket.Datos.DAO;
using UniMarket.Datos.Repository;
using UniMarket.Entidades;

namespace UniMarket.Logica
{
    public class PublicacionService
    {
        private const decimal PrecioMaximo = 9999999999.99m;

        private readonly IPublicacionRepository _repository;

        public PublicacionService(IPublicacionRepository repository)
            => _repository = repository;

        // Constructor de conveniencia
        public PublicacionService()
            : this(new PublicacionRepository(new PublicacionDAO())) { }

        public int Publicar(Publicacion p)
        {
            Limpiar(p);

            if (p.VendedorId <= 0)
                throw new Exception("Debes iniciar sesión para publicar.");

            ValidarDatos(p);

            if (p.Stock < 1)
                throw new Exception("Debes tener al menos 1 unidad para publicar.");

            return _repository.Add(p);
        }

        public void Editar(Publicacion p, int usuarioId)
        {
            Limpiar(p);

            var existente = ObtenerPropia(p.Id, usuarioId);
            ValidarDatos(p);

            p.VendedorId = existente.VendedorId;
            _repository.Update(p);
        }

        public void Eliminar(int id, int usuarioId)
        {
            ObtenerPropia(id, usuarioId);
            _repository.Delete(id);
        }

        public List<Publicacion> ObtenerCatalogo(string texto)
        {
            texto = (texto ?? "").Trim();

            return texto == ""
                ? _repository.GetAvailable()
                : _repository.Search(texto);
        }

        public List<Publicacion> ObtenerMisPublicaciones(int vendedorId)
        {
            if (vendedorId <= 0)
                throw new Exception("Debes iniciar sesión para ver tus publicaciones.");

            return _repository.GetByVendedor(vendedorId);
        }

        private Publicacion ObtenerPropia(int id, int usuarioId)
        {
            if (id <= 0)
                throw new Exception("La publicación no es válida.");

            var existente = _repository.GetById(id);

            if (existente == null || !existente.Activo)
                throw new Exception("La publicación no existe.");

            if (!existente.PerteneceA(usuarioId))
                throw new Exception("Solo el vendedor puede modificar esta publicación.");

            return existente;
        }

        private void Limpiar(Publicacion publicacion)
        {
            if (publicacion == null)
                throw new Exception("Los datos de la publicación son obligatorios.");

            publicacion.Titulo = (publicacion.Titulo ?? "").Trim();

            string descripcion = (publicacion.Descripcion ?? "").Trim();
            publicacion.Descripcion = descripcion == "" ? null : descripcion;
        }

        private void ValidarDatos(Publicacion publicacion)
        {
            if (publicacion.Titulo == "")
                throw new Exception("El título es obligatorio.");

            if (publicacion.Titulo.Length > 120)
                throw new Exception("El título no puede superar los 120 caracteres.");

            if (publicacion.Descripcion != null && publicacion.Descripcion.Length > 500)
                throw new Exception("La descripción no puede superar los 500 caracteres.");

            if (publicacion.Precio <= 0)
                throw new Exception("El precio debe ser mayor a cero.");

            if (publicacion.Precio > PrecioMaximo)
                throw new Exception("El precio es demasiado alto.");

            if (publicacion.Stock < 0)
                throw new Exception("El stock no puede ser negativo.");
        }
    }
}