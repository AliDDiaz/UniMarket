using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniMarket.Datos.DAO
{
    public class Conexion
    {

        private readonly string _cadena;

        public Conexion()
        {

            _cadena = ConfigurationManager.ConnectionStrings["UnimarketDB"].ConnectionString;

        }

        public NpgsqlConnection ObtenerConexion()
            => new NpgsqlConnection(_cadena);

    }
}
