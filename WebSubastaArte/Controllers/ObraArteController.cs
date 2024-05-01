using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSubastaArte.Models;
using static System.Collections.Specialized.BitVector32;
using System.IO;
using System.Reflection;

namespace WebSubastaArte.Controllers
{
    public class ObraArteController : Controller
    {
        // GET: ObraArte
        public ActionResult RegistrarObra()
        {
            // Obtener el nombre de usuario de la sesión
            string nombreUsuario = (string)Session["Usuario"];
            string tipoUsuario = (string)Session["tipoUsuario"];

            var usuario = new Usuario { Email = nombreUsuario, TipoUsuario = tipoUsuario };

            // Pasar el objeto Usuario a la vista
            return View(usuario);


            //validar si el usuario está conectado
            if (usuario.Email == null)
            {

                return RedirectToAction("Acceder", "Login");
            }
            else
            {
              return View(usuario);
            
            }
  
        }

        [HttpPost]
        public ActionResult RegistrarObra(FormCollection form)
        {

            int id = ((Usuario)Session["UserId"]).IdUsuario;
            string categoria = form["categoria"];
            string titulo = form["titulo"];
            string descripcion = form["descripcion"];
            int dimensionLargo = Convert.ToInt32(form["dimensionLargo"]);
            int dimensionAncho = Convert.ToInt32(form["dimensionLargo"]);


            // Acceder al archivo de imagen cargado
            HttpPostedFileBase imagen = Request.Files["Imagen"];

                byte[] imagenData = null;
                using (var binaryReader = new BinaryReader(imagen.InputStream))
                {
                    imagenData = binaryReader.ReadBytes(imagen.ContentLength);
                }


            // Guardado de los datos en la base de datos
            using (SqlConnection con = DBConexion.ObtenerConeccion())
            {
                con.Open();
                SqlCommand command = new SqlCommand("GuardarObra", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@IdUsuario", id);
                command.Parameters.AddWithValue("@Categoria", categoria);
                command.Parameters.AddWithValue("@Titulo", titulo);
                command.Parameters.AddWithValue("@Descripcion", descripcion);
                command.Parameters.AddWithValue("@DimensionLargo", dimensionLargo);
                command.Parameters.AddWithValue("@DimensionAncho", dimensionAncho);
                command.Parameters.AddWithValue("@Imagen", imagenData);
                command.ExecuteNonQuery();
            }

            //return View();
            return RedirectToAction("RegistrarSubasta", "Subasta");

        }


        public ActionResult MostrarObra()
        {

            string nombreUsuario = (string)Session["Usuario"];
            var usuario = new Usuario { Email = nombreUsuario };

            int idUsuario = ((Usuario)Session["UserId"]).IdUsuario;


            if (usuario.Email != null)
            {
                DataTable obraTable = new DataTable();

                using (SqlConnection con = DBConexion.ObtenerConeccion())
                {
                    con.Open();
                    string query = "SELECT * FROM ObraArte WHERE IdUsuario = @IdUsuario";

                    SqlCommand command = new SqlCommand(query, con);
                    command.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);

                    adapter.Fill(obraTable);



                    // Convertir la imagen en bytes a una cadena Base64
                   // foreach (DataRow row in obraTable.Rows)
                    //{
                      //  byte[] imageData = (byte[])row["Imagen"];
                        //string base64String = Convert.ToBase64String(imageData);
                       // row["Imagen"] = base64String;
                        //}


                    return View(obraTable);
                    //return View(obraTable,usuario.Email);
                }
            }
            else
            {
                return RedirectToAction("Acceder", "Login");
            }
        }


    }
}
