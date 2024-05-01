using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace WebSubastaArte.Models
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