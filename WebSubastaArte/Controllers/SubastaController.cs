using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSubastaArte.Models;

namespace WebSubastaArte.Controllers
{
    public class SubastaController : Controller
    {
        // GET: Subasta
        public ActionResult RegistrarSubasta(int id)
        {

            // Obtener el nombre de usuario de la sesión
            string nombreUsuario = (string)Session["Usuario"];
            string tipoUsuario = (string)Session["tipoUsuario"];

            var usuario = new Usuario { Email = nombreUsuario, TipoUsuario = tipoUsuario };

            //validar si el usuario está conectado
            if (usuario.Email == null)
            {

                return RedirectToAction("Acceder", "Login");
            }
            else
            {

                ObraArte _obraArte = ObtenerObraArteDesdeBD(id);

                string emailUsuario = (string)Session["Usuario"];

                ViewBag.Email = emailUsuario;
                ViewBag.TipoUsuario = tipoUsuario;

                ViewBag.Id = _obraArte.Id;
                ViewBag.Titulo = _obraArte.Titulo;
                ViewBag.Descripcion = _obraArte.Descripcion;
                ViewBag.Imagen = _obraArte.Imagen;


                // Pasar el objeto Usuario a la vista
                return View(usuario);
            }
        }

        [HttpPost]
        public ActionResult RegistrarSubasta()
        {


            return View();
        }


        private ObraArte ObtenerObraArteDesdeBD(int id)
        {
            string query = "SELECT Id, Titulo, Descripcion, Imagen" +
                " FROM ObraArte WHERE Id = @Id";

            ObraArte _obraArte = new ObraArte();

            using (SqlConnection con = DBConexion.ObtenerConeccion())
            {
                con.Open();
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@Id", id);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    _obraArte.Id = (int)reader["Id"];
                    _obraArte.Titulo = reader["Titulo"].ToString();
                    _obraArte.Descripcion = reader["Titulo"].ToString();
                    _obraArte.Imagen = (byte[])reader["Imagen"];
                }
                reader.Close();
            }
            return _obraArte;
        }
    }

}