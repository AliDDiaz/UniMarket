using System;
using System.Collections.Generic;
using System.Linq;
using UniMarket.Entidades;

namespace UniMarket.Logica
{
    public static class CarritoService
    {
        private static readonly List<ItemCarrito> _items = new List<ItemCarrito>();

        public static IReadOnlyList<ItemCarrito> ObtenerItems() => _items.AsReadOnly();

        public static int TotalUnidades => _items.Sum(i => i.Cantidad);

        public static decimal TotalPagar => _items.Sum(i => i.Subtotal);
        public static event Action OnCarritoActualizado;

        public static void Agregar(Publicacion pub, int cantidad = 1)
        {
            if (pub == null || cantidad <= 0) return;
            if (!pub.AceptaVenta)
                throw new Exception($"\"{pub.Titulo}\" solo se ofrece por trueque; no se puede agregar al carrito.");

            var existente = _items.FirstOrDefault(i => i.Producto.Id == pub.Id);
            if (existente != null)
            {
                if (existente.Cantidad + cantidad > pub.Stock)
                    throw new Exception($"Solo hay {pub.Stock} unidades disponibles de \"{pub.Titulo}\".");

                existente.Cantidad += cantidad;
            }
            else
            {
                if (cantidad > pub.Stock)
                    throw new Exception($"Solo hay {pub.Stock} unidades disponibles.");

                _items.Add(new ItemCarrito { Producto = pub, Cantidad = cantidad });
            }
            OnCarritoActualizado?.Invoke();
        }

        public static void CambiarCantidad(int publicacionId, int nuevaCantidad)
        {
            var item = _items.FirstOrDefault(i => i.Producto.Id == publicacionId);
            if (item == null) return;

            if (nuevaCantidad <= 0)
            {
                _items.Remove(item);
                OnCarritoActualizado?.Invoke();
                return;
            }

            if (nuevaCantidad > item.Producto.Stock)
                throw new Exception($"Stock máximo disponible: {item.Producto.Stock}.");

            item.Cantidad = nuevaCantidad;
            OnCarritoActualizado?.Invoke();
        }

        public static void Eliminar(int publicacionId)
        {
            _items.RemoveAll(i => i.Producto.Id == publicacionId);
            OnCarritoActualizado?.Invoke();
        }

        public static void Vaciar()
        {
            _items.Clear();
            OnCarritoActualizado?.Invoke();
        }
    }
}