using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebFitnessProy1.Models
{
    public class DBConexion
    {
        public static SqlConnection ObtenerConeccion()
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["ConexionBD"].ConnectionString;
            return new SqlConnection(cadenaConexion);
        }
    }
}